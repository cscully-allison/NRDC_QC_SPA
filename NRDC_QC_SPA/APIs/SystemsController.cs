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
            List<object> formattedRows = new List<object>();

            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Systems
                                select x;

                    foreach (var row in table)
                    {
                        formattedRows.Add(new
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

        //return a single system by
        // PK
        public HttpResponseMessage Get(string DBName, int id)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;


            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var row = (from x in db.Systems
                               where x.System == id
                               select x).FirstOrDefault();
                    var returned = new
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


        //get by systemID
        [Route("api/Systems/GetSiteSystems/{DBName}/{SiteId}/")]
        public HttpResponseMessage GetSiteSystems(string DBName, int SiteId)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            List<object> formattedRows = new List<object>();


            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Systems
                                where x.Site == SiteId
                                select x;

                    foreach (var row in table)
                    {

                        formattedRows.Add(
                            new {
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
