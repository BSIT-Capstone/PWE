using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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