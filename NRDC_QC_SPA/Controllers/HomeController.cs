using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NRDC_QC_SPA.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        { 
            ViewBag.databaseOptions = new string [] {"ProtoNRDC", "NRDC"};
            ViewBag.Title = "NRDC";
            return View();
        }

        public ActionResult GraphView()
        {
            ViewBag.item = "wooo";

            return PartialView("~/Views/Home/_GraphArea.cshtml");
        }

        public ActionResult OptionsView()
        { 
            return PartialView("~/Views/Home/_Options.cshtml");
        }
    }
}
