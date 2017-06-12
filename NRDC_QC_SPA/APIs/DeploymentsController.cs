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
            List<object> formattedRows = new List<object>();

            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Deployments
                                select x;
                    foreach (var row in table)
                    {
                        formattedRows.Add(new
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

        //return a single deployment by
        // PK
        public HttpResponseMessage Get(string DBName, int id)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;


            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var row = (from x in db.Deployments
                               where x.Deployment == id
                               select x).FirstOrDefault();
                    var returned = new
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
        [Route("api/Deployments/GetSystemDeployments/{DBName}/{SystemId}/")]
        public HttpResponseMessage GetSystemDeployments(string DBName, int SystemId)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            List<object> formattedRows = new List<object>();


            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Deployments
                               where x.System == SystemId
                               select x;

                    foreach (var row in table)
                    {

                        formattedRows.Add(
                            new {
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
