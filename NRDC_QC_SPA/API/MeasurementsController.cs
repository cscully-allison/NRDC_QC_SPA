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
        //GET all Measurements (can def hurt)
        public HttpResponseMessage Get(string DBName)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            List<object> formattedRows = new List<object>();
            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = (from x in db.Measurements
                                select x).Take(1000);
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
        //id is the data stream id
        //not really useful either needs bounds on time
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

        public HttpResponseMessage Get(string DBName, int DataStreamId, DateTime StartTime, DateTime EndTime)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            List<object> queriedMeasurements = new List<object>();

            try{
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    //join queried measurements with data streams to retrieve data on units
                    // I think I need another join?
                    // I think in streams I do
                    var table = from measurements in db.Measurements
                                join data_streams in db.Data_Streams on measurements.Stream equals data_streams.Stream
                                join units in db.Units on data_streams.Unit equals units.Unit
                                where measurements.Stream == DataStreamId
                                && measurements.Measurement_Time_Stamp > StartTime
                                && measurements.Measurement_Time_Stamp <= EndTime
                                select new { measurements, units.Name, units.Abbreviation };

                    foreach (var row in table)
                    {
                        queriedMeasurements.Add(new
                        {
                            DataStreamID = row.measurements.Stream,
                            TimeStamp = row.measurements.Measurement_Time_Stamp,
                            Value = row.measurements.Value,
                            ControlledValue = row.measurements.Controlled_Value,
                            L1FlagID = row.measurements.L1_Flag,
                            L2FlagID = row.measurements.L2_Flag,
                            UnitName = row.Name,
                            UnitAbbrevation = row.Abbreviation
                        });
                    }
                }

                json = JsonConvert.SerializeObject(queriedMeasurements);

            }
            catch(Exception e)
            {
                json = "Error: " + e.Message;
            }

            return conn.BuildJsonResponse(json);
        }


    }
}
