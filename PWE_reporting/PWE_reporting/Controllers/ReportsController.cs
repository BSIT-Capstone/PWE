using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PWE_reporting.Models;
using PWE_reporting.ReportWebService;
using PWE_reporting.ReportWebReference;
using System.Web.Services.Protocols;
using System.Reflection;
using System.IO;

namespace PWE_reporting.Controllers
{

    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Reports()
        {
            string path = Server.MapPath("ReportReferences/ReportReferences.txt");
            string[] filetext = System.IO.File.ReadAllLines(path);
            string reportservice, reportexecution = "";
            reportservice = filetext[0];
            reportexecution = filetext[1];
            int index = 0;
            index = reportservice.IndexOf("=");
            if(index > 0)
            {
                reportservice = reportservice.Substring(index+1);
            }
            index = reportexecution.IndexOf("=");
            if (index > 0)
            {
                reportexecution = reportexecution.Substring(index+1);
            }
                                                         //reportservice   reportexecution
            ReportReferences refer = new ReportReferences(reportservice, reportexecution);
            ReportingService2005 rr = new ReportingService2005();
            //rr.Credentials = new System.Net.NetworkCredential("username", "password");
            rr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            rr.Url = refer._reportservice;

            CatalogItem[] items = null;

            try
            {
                items = rr.ListChildren("/PWE_Reports", true);
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
                string reportName = "/PWE_Reports/" + report.ReportName;
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
            string path = Server.MapPath("../ReportReferences/ReportReferences.txt");
            string[] filetext = System.IO.File.ReadAllLines(path);
            string reportservice, reportexecution = "";
            reportservice = filetext[0];
            reportexecution = filetext[1];
            int index = 0;
            index = reportservice.IndexOf("=");
            if (index > 0)
            {
                reportservice = reportservice.Substring(index + 1);
            }
            index = reportexecution.IndexOf("=");
            if (index > 0)
            {
                reportexecution = reportexecution.Substring(index + 1);
            }
            //reportservice   reportexecution
            ReportReferences refer = new ReportReferences(reportservice, reportexecution);
            ReportExecutionService rs = new ReportExecutionService();
            //rs.Credentials = new System.Net.NetworkCredential("username", "password");
            rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
            rs.Url = refer._reportexec;

            string encoding = null;
            string mimeType = null;
            string[] streamIDs = null;
            string devInfo = "<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";


            string reportPath = null;
            reportPath = "/PWE_Reports/" + reportname;
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
            if (productid != null) //report has a pricegroupid parameter
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

            /*
                       ViewBag.ReportName = reportname;
                       ViewBag.ReportParam = pricegroupid;
                       return View();
                       */

        }



    }
    }