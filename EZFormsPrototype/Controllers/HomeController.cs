﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EZFormsPrototype.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Form");
        }

        public ActionResult About()
        {
            ViewBag.Message = "What is EZ Forms?";

            return View();
        }
    }
}