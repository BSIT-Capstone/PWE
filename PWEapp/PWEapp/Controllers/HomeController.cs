﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PWEapp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Help()
        {
           // ViewBag.Message = "PWE Help page.";

            return View();
        }

        public ActionResult Reports()
        {
            ViewBag.Message = "PWE Reports page";

            return View();
        }
    }
}