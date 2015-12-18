using System.Collections.Generic;
/*
Class used to store values for reports from reporting services.
Values depend on report configurations from reporting services. 
ReportName is the name of the file in reporting services, 
ReportDescription is the description set for report in reporting services,
ParameterName is the name of the parameter (Price Group ID) for report from reporting services,
etc... 
*/
namespace PWE_reporting.Models
{
    public class Report
    {
        public Report()
        {
            this.ReportParameters = new List<Parameter>();
            this.DataParameters = new List<Parameter>();
        }
        public string ReportName { get; set; } 
        public string ReportDescription { get; set; } 
        public List<Parameter> ReportParameters { get; set; }
        public List<Parameter> DataParameters { get; set; }
    }

    public class Parameter
    {
        public string ParameterName { get; set; }
        public string Prompt { get; set; }
        public string Value { get; set; }
    }

}