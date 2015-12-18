using System.Collections.Generic;
using System.Web.Mvc;
using PWE_reporting.Models;
using PWE_reporting.ReportWebService;
using PWE_reporting.ReportWebReference;
using System.Web.Services.Protocols;
using System.Configuration;
/*
By: Greg Carlson
Controller class that handles Reports Page and Downloading of reports. Users are authenticated with their Windows credentials
for accessing and downloading the reports from reporting services server that are hosted from a company domain. To use the 
Controller class the ReportService, ReportExecution, and ReportPath need to be set in Web.config. For configuration information
refer to the PWE install guide. 
*/
namespace PWE_reporting.Controllers
{

    public class ReportsController : Controller
    {
        /* Reports homepage with form to download reports from reporting services. The page displays Report Name, Report Description 
           and Report Prompt along with parameters for Price Group ID and Product ID that are configured for the reports in 
           reporting services. 
        */
        public ActionResult Reports()
        {
            //URL for ReportService set in Web.config  
            string reportService = ConfigurationManager.AppSettings["ReportService"].ToString();
            //reporting services object
            ReportingService2005 rr = new ReportingService2005(); 
            //authentication from Windows user account
            rr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            //connectiong to ReportService
            rr.Url = reportService;

            //Path to reports on reporting services dashboard set in Web.config 
            string reportFolder = ConfigurationManager.AppSettings["ReportFolder"].ToString();
            CatalogItem[] items = null;
            try
            {
                items = rr.ListChildren("/"+reportFolder, true); //read reports into array
            }
            catch (SoapException e)
            {
                throw e;
            }

            var reports = new List<Report>();

            foreach (CatalogItem value in items)
            {   //create reports using Reports class.   Name of report file     and     report description when report was created using reporting services
                reports.Add(new Report() { ReportName = value.Name, ReportDescription = value.Description });
            }

            ReportExecutionService rs = new ReportExecutionService();


            //loop through reports 
            foreach (var report in reports)
            {
                string reportName = "/"+reportFolder+"/" + report.ReportName;
                bool forRender = true;
                string historyID = null;
                ReportWebReference.ParameterValue[] values = null;
                ReportWebReference.DataSourceCredentials[] credentials = null;
                ReportWebReference.ReportParameter[] parameters = null;

                //check if they have parameters price group id or product id set for reports on reporting services
                try
                {
                    parameters = rr.GetReportParameters(reportName, historyID, forRender, values, credentials);

                    if (parameters != null)
                    {   //add parameter values to report objects using Reports Class 
                        foreach (ReportWebReference.ReportParameter rp in parameters)
                        {                                        //report parameter name (Price Group ID) and prompt from reporting services
                            report.ReportParameters.Add(new Parameter() { ParameterName = rp.Name, Prompt = rp.Prompt });

                            //look for values for each parameter and add them to parameter list of values
                            if (rp.ValidValues != null)
                            {
                                foreach (var validValue in rp.ValidValues)
                                {
                                    report.DataParameters.Add(new Parameter() { Value = validValue.Value });
                                }
                            }
                        }

                    }
                }
                catch (SoapException e)
                {
                    throw e;
                }
            }

            
            return View(reports);
        }
        //Form action for downloading report from Reports Home page. 
        public ActionResult DownloadReport(string reportname, string pricegroupid, string productid)
        {   
            //create report execution service object
            ReportExecutionService rs = new ReportExecutionService();
            //authentication from Windows user account
            rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
            //URL for ReportExecution service set in Web.config  
            string reportExec = ConfigurationManager.AppSettings["ReportExecution"].ToString();
            //connectiong to ReportExecutionService
            rs.Url = reportExec;
         
            //create an EXCEL file for download
            string encoding = null;
            string mimeType = null;
            string[] streamIDs = null;
            string devInfo = "<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";

            //read reports from reporting services directory
            string reportPath = null;
            //Path to report location set in Web.config
            string reportFolder = ConfigurationManager.AppSettings["ReportFolder"].ToString();
            reportPath = "/" + reportFolder + "/" + reportname;
            
            //create values for creating EXCEL file
            string historyID = null;
            byte[] result = null;
            string format = "EXCEL";
            string extension = null;

            ReportWebService.Warning[] warnings = null;
            ExecutionInfo execInfo = new ExecutionInfo();
            ExecutionHeader execHeader = new ExecutionHeader();
            //set header for file
            Response.AddHeader("Content-Disposition", "inline; filename=" + reportname + ".xls");
            rs.ExecutionHeaderValue = execHeader;
            //set parameter value for report execution
            ReportWebService.ParameterValue pv = new ReportWebService.ParameterValue();
            execInfo = rs.LoadReport(reportPath, historyID);
            //check reports for values
            ReportWebService.ParameterValue[] parameters = new ReportWebService.ParameterValue[1] { pv };
            if (pricegroupid != null) //report has a pricegroupid parameter
            {
                pv.Name = "PriceGroupID";
                pv.Value = pricegroupid;
                rs.SetExecutionParameters(parameters, "en-us");
            }
            if (productid != null) //report has a ProductID parameter
            {
                pv.Name = "ProductID";
                pv.Value = productid;
                rs.SetExecutionParameters(parameters, "en-us");
            }
            //render and execute report 
            try
            {
                result = rs.Render(format, devInfo, out extension, out mimeType, out encoding, out warnings, out streamIDs);

                execInfo = rs.GetExecutionInfo();
            }
            catch (SoapException e)
            {
                throw e;
            }
            //download report to the web browser on a new tab
            return File(result, "application/vnd.ms-excel");
        }
    }
 }