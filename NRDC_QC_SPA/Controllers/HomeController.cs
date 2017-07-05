﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NRDC.Projects.Models.GIDMIS;
using NRDC_QC.APIs;
using NRDC_QC_SPA.Models;

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

        //partial view on main page is sent through here
        // so I can query for flag data and even specific user data in this controller
        public ActionResult Dashboard()
        {
            ConnectionHelper conn = new ConnectionHelper();
            List<object> flags = new List<object>();


            using (var db = new GIDMISContainer(conn.getConnectionString("protoNRDC")))
            {
                var table = (from MES in db.Measurements
                             join ONEQC in db.L1_Quality_Control on MES.L1_Flag equals ONEQC.Flag
                             where
                                MES.L1_Flag != null
                             && MES.Controlled_Value == null
                             && MES.Data_Streams.Data_Interval.Minutes == 10
                             select new { MES, ONEQC } ).Take(100);

                foreach (var row in table)
                {
                    flags.Add(new FlagViewModel() {
                        QCLV1 = row.MES.L1_Flag,
                        QCLV2 = row.MES.L2_Flag,
                        DataStreamId = row.MES.Stream,
                        TimeStamp = row.MES.Measurement_Time_Stamp,
                        Deployment = row.MES.Data_Streams.Deployments.Name,
                        System = row.MES.Data_Streams.Deployments.Systems.Name,
                        Site = row.MES.Data_Streams.Deployments.Systems.Sites.Name,
                        SiteAlias = row.MES.Data_Streams.Deployments.Systems.Sites.Alias,
                        FlagName = row.MES.L1_Quality_Control.Name
                    });
                }


                ViewBag.flags = flags;

            }



            return PartialView("~/Views/Home/_Dashboard.cshtml");



        }

    }
}
