using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NRDC_QC_SPA.Controllers
{
    public class ChartViewController : Controller
    {
        // GET: ChartView
        public ActionResult Index()
        {
            ViewBag.data = "From C# code";
            return View();
        }

    }
}