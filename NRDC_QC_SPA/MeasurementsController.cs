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
    public class MeasurementsController : ApiController
    {
        //GET all Measurements (kinda pointless but can't hurt)
        public HttpResponseMessage Get(string DBName)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            List<object> formattedRows = new List<object>();
            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Measurements
                                select x;
                    foreach (var row in table)
                    {
                        formattedRows.Add(new
                        {
                            DataStreamID = row.Stream,
                            TimeStamp = row.Measurement_Time_Stamp,
                            Value = row.Value,
                            ControlledValue = row.Controlled_Value,
                            L1FlagID = row.L1_Flag,
                            L2FlagID = row.L2_Flag
                        });
                    }
                }

                json = JsonConvert.SerializeObject(formattedRows);

            }
            catch (Exception e)
            {
                json = "Error: " + e.Message;
            }

            return conn.BuildJsonResponse(json);
        }


        //GET all measurements from a particular stream
        //id is the data stream id in this case
        public HttpResponseMessage Get(string DBName, int id)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            List<object> formattedRows = new List<object>();
            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Measurements
                                where x.Stream == id
                                select x;
                    foreach (var row in table)
                    {
                        formattedRows.Add(new
                        {
                            DataStreamID = row.Stream,
                            TimeStamp = row.Measurement_Time_Stamp,
                            Value = row.Value,
                            ControlledValue = row.Controlled_Value,
                            L1FlagID = row.L1_Flag,
                            L2FlagID = row.L2_Flag
                        });
                    }
                }

                json = JsonConvert.SerializeObject(formattedRows);

            }
            catch (Exception e)
            {
                json = "Error: " + e.Message;
            }

            return conn.BuildJsonResponse(json);
        }

        //GET a single measurement
        //Requires DataStreamId and TimeStamp
        //Security issues with time sramp
        /*
        [Route("api/Measurements/{DBName}/{DataStreamId}/{TimeStamp}")]
        public HttpResponseMessage Get(string DBName, int DataStreamId, DateTime TimeStamp)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            object measurement;

            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var row = (from x in db.Measurements
                                where (x.Stream == DataStreamId &&
                                        x.Measurement_Time_Stamp == TimeStamp)
                                select x).FirstOrDefault();
                   
                    measurement = new {
                        DataStreamID = row.Stream,
                        TimeStamp = row.Measurement_Time_Stamp,
                        Value = row.Value,
                        ControlledValue = row.Controlled_Value,
                        L1FlagID = row.L1_Flag,
                        L2FlagID = row.L2_Flag
                    };
                   
                }

                json = JsonConvert.SerializeObject(measurement);

            }
            catch (Exception e)
            {
                json = "Error: " + e.Message;
            }

            return conn.BuildJsonResponse(json);
        }
        */

    }
}
