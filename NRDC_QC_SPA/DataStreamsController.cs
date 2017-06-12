using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.Http;
using System.Diagnostics;
using NRDC_QC.APIs;
using NRDC.Projects.Models.GIDMIS;
using Newtonsoft.Json;
using NRDC_QC.APIs.DataTransferModels;
using System.Net.Http;




namespace NRDC_QC.Models
{
    public class DataStreamsController : ApiController
    {
        // GET: api/DataStreams
        // Deafult no db arg connects to
        // ProtoNRDC presently
        public HttpResponseMessage Get()
        {
            ConnectionHelper conn = new ConnectionHelper();
            string NRDCConnStr, JSON;
            List<DataStream> formattedRows = new List<DataStream>();

            //connection and setup
            NRDCConnStr = conn.getConnectionString("ProtoNRDC");


            //query recovered entity
            using (var db = new GIDMISContainer(NRDCConnStr))
            {

                var table = from x in db.Data_Streams
                            select x;

                foreach (var row in table)
                {
                    DataStream stream = new DataStream(row);

                    formattedRows.Add(stream);
                }

                JSON = JsonConvert.SerializeObject(formattedRows);
                var Response = this.Request.CreateResponse(System.Net.HttpStatusCode.OK);
                Response.Content = new StringContent(JSON, System.Text.Encoding.UTF8, "application/json");
                return Response;
            }

        }

        // GET: api/DataStreams/5
        public HttpResponseMessage Get(string DBName)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string NRDCConnStr, JSON;
            List<DataStream> formattedRows = new List<DataStream>();
            

            try
            {
                //connection and setup
                NRDCConnStr = conn.getConnectionString(DBName);

                //query recovered entity
                using (var db = new GIDMISContainer(NRDCConnStr))
                {

                    var table = from x in db.Data_Streams
                                select x;

                    foreach (var row in table)
                    {
                        DataStream stream = new DataStream(row);

                        formattedRows.Add(stream);

                    }

                    JSON = JsonConvert.SerializeObject(formattedRows);
                }
            }
            catch(Exception e)
            {
                JSON = JsonConvert.SerializeObject("Error: " + e.Message);
            }

            var Response = this.Request.CreateResponse(System.Net.HttpStatusCode.OK);
            Response.Content = new StringContent(JSON, System.Text.Encoding.UTF8, "application/json");
            return Response;
        }

        //GET: A single entity where id is the PK
        //      and DBName is the utilized database name
        public HttpResponseMessage Get(string DBName, int id)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string NRDCConnStr, JSON;

            try
            {
                //connection and setup
                NRDCConnStr = conn.getConnectionString(DBName);


                //query recovered entity
                using (var db = new GIDMISContainer(NRDCConnStr))
                {

                    var row = (from x in db.Data_Streams
                              where x.Stream == id
                              select x).FirstOrDefault();

                    var stream = new DataStream(row);

                    JSON = JsonConvert.SerializeObject(stream);
                }
            }
            catch (Exception e)
            {
                JSON = JsonConvert.SerializeObject("Error: " + e.Message);

            }

            var Response = this.Request.CreateResponse(System.Net.HttpStatusCode.OK);
            Response.Content = new StringContent(JSON, System.Text.Encoding.UTF8, "application/json");
            return Response;
        }

        // POST: api/DataStreams
        // Prereq: transmitted data must be of the type
        //          application/json
        // Ajax example: $.ajax({contentType: "application/json"})
        public void Post(string DBName, [FromBody]DataStream value)
        {
            //these should be auto uploaded without human intervention
            Debug.WriteLine("this is called");
            Debug.WriteLine(value.DeploymentID);
        }

        // PUT: api/DataStreams/5
        public void Put(int id, [FromBody]string value)
        {
            //corrected data
        }

        // DELETE: api/DataStreams/5
        public void Delete(int id)
        {
            //I dont think we want an interface to delete either 
        }
    }
}
