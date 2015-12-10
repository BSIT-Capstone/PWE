using System.Collections.Generic;
using System.Web.Mvc;
using PWE_reporting.Models;
using PWE_reporting.ReportWebService;
using PWE_reporting.ReportWebReference;
using System.Web.Services.Protocols;
using System.Configuration;

namespace PWE_reporting.Controllers
{

    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Reports()
        {
        
            string reportService = ConfigurationManager.AppSettings["ReportService"].ToString();
            ReportingService2005 rr = new ReportingService2005();
            rr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            rr.Url = reportService;

            string reportFolder = ConfigurationManager.AppSettings["ReportFolder"].ToString();
            CatalogItem[] items = null;
            try
            {
                items = rr.ListChildren("/"+reportFolder, true);
            }
            catch (SoapException e)
            {
                throw e;
            }

            var reports = new List<Report>();

            foreach (CatalogItem value in items)
            {
                reports.Add(new Report() { ReportName = value.Name, ReportDescription = value.Description });
            }

            ReportExecutionService rs = new ReportExecutionService();

            foreach (var report in reports)
            {
                string reportName = "/"+reportFolder+"/" + report.ReportName;
                bool forRender = true;
                string historyID = null;
                ReportWebReference.ParameterValue[] values = null;
                ReportWebReference.DataSourceCredentials[] credentials = null;
                ReportWebReference.ReportParameter[] parameters = null;

                try
                {
                    parameters = rr.GetReportParameters(reportName, historyID, forRender, values, credentials);

                    if (parameters != null)
                    {
                        foreach (ReportWebReference.ReportParameter rp in parameters)
                        {
                            report.ReportParameters.Add(new Parameter() { ParameterName = rp.Name, Prompt = rp.Prompt });

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

        public ActionResult DownloadReport(string reportname, string pricegroupid, string productid)
        {
            ReportExecutionService rs = new ReportExecutionService();
            rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
            string reportExec = ConfigurationManager.AppSettings["ReportExecution"].ToString();
            rs.Url = reportExec;
         
            string encoding = null;
            string mimeType = null;
            string[] streamIDs = null;
            string devInfo = "<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";


            string reportPath = null;
            string reportFolder = ConfigurationManager.AppSettings["ReportFolder"].ToString();
            reportPath = "/" + reportFolder + "/" + reportname;
            string historyID = null;
            byte[] result = null;
            string format = "EXCEL";
            string extension = null;

            ReportWebService.Warning[] warnings = null;
            ExecutionInfo execInfo = new ExecutionInfo();
            ExecutionHeader execHeader = new ExecutionHeader();

            Response.AddHeader("Content-Disposition", "inline; filename=" + reportname + ".xls");
            rs.ExecutionHeaderValue = execHeader;

            ReportWebService.ParameterValue priceGroupID = new ReportWebService.ParameterValue();
            execInfo = rs.LoadReport(reportPath, historyID);

            ReportWebService.ParameterValue[] parameters = new ReportWebService.ParameterValue[1] { priceGroupID };
            if (pricegroupid != null) //report has a pricegroupid parameter
            {   
                priceGroupID.Name = "PriceGroupID";
                priceGroupID.Value = pricegroupid;
                rs.SetExecutionParameters(parameters, "en-us");
            }
            if (productid != null) //report has a ProductID parameter
            {
                priceGroupID.Name = "ProductID";
                priceGroupID.Value = productid;
                rs.SetExecutionParameters(parameters, "en-us");
            }

            try
            {
                result = rs.Render(format, devInfo, out extension, out mimeType, out encoding, out warnings, out streamIDs);

                execInfo = rs.GetExecutionInfo();
            }
            catch (SoapException e)
            {
                throw e;
            }

            return File(result, "application/vnd.ms-excel");

        }



    }
    }