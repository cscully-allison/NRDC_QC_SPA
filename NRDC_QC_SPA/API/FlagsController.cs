using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NRDC.Projects.Models.GIDMIS;
using Newtonsoft.Json;

namespace NRDC_QC.APIs
{
    public class FlagsController : ApiController
    {
        // /{dbName}/flags/
        public HttpResponseMessage Get(string DBName)
        {
            ConnectionHelper conn = new ConnectionHelper();
            List<object> flags = new List<object>();
            string json;


            using (var db = new GIDMISContainer(conn.getConnectionString("protoNRDC")))
            {
                var table = (from MES in db.Measurements
                             join ONEQC in db.L1_Quality_Control on MES.L1_Flag equals ONEQC.Flag
                             where
                                MES.L1_Flag != null
                             && MES.Controlled_Value == null
                             && MES.Data_Streams.Data_Interval.Minutes == 10
                             select new { MES, ONEQC }).Take(100);

                foreach (var row in table)
                {
                    flags.Add(new{
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

                json = JsonConvert.SerializeObject(flags);

                return conn.BuildJsonResponse(json);
            }

        }
    }
}
