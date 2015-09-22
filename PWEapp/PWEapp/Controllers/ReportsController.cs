using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PWEapp.Models;

namespace PWEapp.Controllers
{
    public class ReportsController : Controller
    {
        private PimDbContext pimDb = new PimDbContext();
        // GET: Reports
        public ActionResult Reports()
        {
            var report_names = pimDb.Reports.ToList();
            return View(report_names);
        }

        public ActionResult LookupTables() {
            ViewBag.Message = "Lookup Tables";
            return View();
        }
    }
}