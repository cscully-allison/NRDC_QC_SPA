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
    public class CategoriesController : ApiController
    {
        //GET: api/Categories/
        //get all categories
        public HttpResponseMessage Get(string DBName)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            List<object> formattedRows = new List<object>();

            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Categories
                                select x;
                    foreach (var row in table)
                    {
                        formattedRows.Add(new
                        {
                            Category = row.Category,
                            Name = row.Name,
                            Description = row.Description
                        });
                    }
                }

                json = JsonConvert.SerializeObject(formattedRows);
            }
            catch(Exception e)
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
                    var row = (from x in db.Categories
                               where x.Category == id
                               select x).FirstOrDefault();
                    var returned = new
                    {
                        Category = row.Category,
                        Name = row.Name,
                        Description = row.Description
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
