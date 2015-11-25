using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PWE_reporting.Models
{
    public class ReportReferences
    {
        public string _reportservice { get; set; }
        public string _reportexec { get; set; }

        public ReportReferences(string reportservice, string reportexec)
        {
            
            _reportservice = reportservice;
            _reportexec = reportexec;
        }
    }
}