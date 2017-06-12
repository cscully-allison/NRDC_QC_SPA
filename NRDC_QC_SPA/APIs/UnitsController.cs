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
    public class UnitsController : ApiController
    {

        //GET: /api/DataTypes/
        public HttpResponseMessage Get(string DBName)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            List<object> formattedRows = new List<object>();
            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Units
                                select x;
                    foreach (var row in table)
                    {
                        formattedRows.Add(new
                        {
                            UnitID = row.Unit,
                            Name = row.Name,
                            Abbreviation = row.Abbreviation,
                            Type = row.Type
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


        //GET
        //Get a category row by id (PK)
        public HttpResponseMessage Get(string DBName, int id)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var row = (from x in db.Units
                               where x.Unit == id
                               select x).FirstOrDefault();
                    var returned = new
                    {
                        UnitID = row.Unit,
                        Name = row.Name,
                        Abbreviation = row.Abbreviation,
                        Type = row.Type
                    };
                    json = JsonConvert.SerializeObject(returned);
                }
            }
            catch (Exception e)
            {
                json = "Error: " + e.Message;
            }
            return conn.BuildJsonResponse(json);
        }
    }
}
