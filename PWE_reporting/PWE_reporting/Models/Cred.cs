using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PWE_reporting.Models
{
    public class Cred
    {
        public string userName { get; set; }
        public string passWord { get; set; }
        public string ReportService { get; set; }
        public string ReportExec { get; set; }

        public Cred(string username, string password, string reportservice, string reportexec)
        {
            userName = username;
            passWord = password;
            ReportService = reportservice;
            ReportExec = reportexec;
        }
    }
}