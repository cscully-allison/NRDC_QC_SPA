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
    public class SystemsController : ApiController
    {
        public HttpResponseMessage Get(string DBName)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            Dictionary<string, object> formattedRows = new Dictionary<string, object>();

            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Systems
                                select x;

                    foreach (var row in table)
                    {
                        formattedRows.Add(row.Name, new
                        {
                            SystemID = row.System,
                            UniqueIdentifier = row.Unique_Identifier,
                            Site = row.Site,
                            Manager = row.Manager,
                            Name = row.Name,
                            Details = row.Details,
                            Power = row.Power,
                            InstallationLocation = row.Installation_Location,
                            CreationDate = row.Creation_Date,
                            ModificationDate = row.Modification_Date,
                            Photo = row.Photo
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

        //return a colelction of systems related to
        //site id
        public HttpResponseMessage Get(string DBName, int id)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            Dictionary<string, object> formattedRows = new Dictionary<string, object>();


            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Systems
                                where x.Site == id
                                select x;

                    foreach (var row in table)
                    {

                        formattedRows.Add(row.Name, new
                        {
                            SystemID = row.System,
                                UniqueIdentifier = row.Unique_Identifier,
                                Site = row.Site,
                                Manager = row.Manager,
                                Name = row.Name,
                                Details = row.Details,
                                Power = row.Power,
                                InstallationLocation = row.Installation_Location,
                                CreationDate = row.Creation_Date,
                                ModificationDate = row.Modification_Date,
                                Photo = row.Photo
                            }
                        );
                    }

                    json = JsonConvert.SerializeObject(formattedRows);
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
