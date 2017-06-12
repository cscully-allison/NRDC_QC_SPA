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
    public class DeploymentsController : ApiController
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
                    var table = from x in db.Deployments
                                select x;
                    foreach (var row in table)
                    {
                        formattedRows.Add(row.Name, new
                        {
                            DeploymentID = row.Deployment,
                            UniqueIDentifier = row.Unique_Identifier,
                            System = row.System,
                            Name = row.Name,
                            Purpose = row.Purpose,
                            CenterOffeset = row.Center_Offset,
                            Location = row.Location,
                            HeightFromGround = row.Height_From_Ground,
                            ParentLogger = row.Parent_Logger,
                            Notes = row.Notes,
                            EstablishedDate = row.Established_Date,
                            AbandonedDate = row.Abandoned_Date,
                            CreationDate = row.Creation_Date,
                            ModificationDate = row.Modification_Date
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

        //return a collection of deployments
        //related to a specific system
        public HttpResponseMessage Get(string DBName, int id)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            Dictionary<string, object> formattedRows = new Dictionary<string, object>();


            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Deployments
                                where x.System == id
                                select x;

                    foreach (var row in table)
                    {

                        formattedRows.Add(row.Name, new
                            {
                                DeploymentID = row.Deployment,
                                UniqueIDentifier = row.Unique_Identifier,
                                System = row.System,
                                Name = row.Name,
                                Purpose = row.Purpose,
                                CenterOffeset = row.Center_Offset,
                                Location = row.Location,
                                HeightFromGround = row.Height_From_Ground,
                                ParentLogger = row.Parent_Logger,
                                Notes = row.Notes,
                                EstablishedDate = row.Established_Date,
                                AbandonedDate = row.Abandoned_Date,
                                CreationDate = row.Creation_Date,
                                ModificationDate = row.Modification_Date
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
