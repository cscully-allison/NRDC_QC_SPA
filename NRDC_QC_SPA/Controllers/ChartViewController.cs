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
        public ActionResult Index(int StreamId, DateTime TimeStamp)
        {
            ViewBag.SID = StreamId;
            ViewBag.StartTime = TimeStamp.AddMonths(-1);
            ViewBag.EndTime = TimeStamp.AddMonths(2);
            

            return PartialView("~/Views/Shared/_ChartView.cshtml");
        }

    }
}