using Newtonsoft.Json;
using NRDC.Projects.Models.GIDMIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NRDC_QC.APIs
{
    public class SiteNetworksController : ApiController
    {
        // GET: api/SiteNetworks/{DBName}
        public HttpResponseMessage Get(string DBName)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            List<object> formattedRows = new List<object>();

            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Site_Networks
                                select x;

                    foreach (var row in table)
                    {
                        formattedRows.Add(new
                        {
                            Network = row.Network,
                            UniqueIdentifier = row.Unique_Identifier,
                            PrincipalInvestigator = row.Principal_Investigator,
                            Name = row.Name,
                            InstituationName = row.Institution_Name,
                            OriginalFunctinAgency = row.Original_Funding_Agency,
                            GrantNumberString = row.Grant_Number_String,
                            StartedDate = row.Started_Date,
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

        //return a single system by
        //PK
        //GET: api/SiteNetworks/ProtoNRDC/
        public HttpResponseMessage Get(string DBName, int id)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;


            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var row = (from x in db.Site_Networks
                               where x.Network == id
                               select x).FirstOrDefault();
                    var returned = new
                    {
                        Network = row.Network,
                        UniqueIdentifier = row.Unique_Identifier,
                        PrincipalInvestigator = row.Principal_Investigator,
                        Name = row.Name,
                        InstituationName = row.Institution_Name,
                        OriginalFunctinAgency = row.Original_Funding_Agency,
                        GrantNumberString = row.Grant_Number_String,
                        StartedDate = row.Started_Date,
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


        //get all site networks associated with a given
        // primary investigator
        [Route("api/SiteNetworks/GetPINetworks/{DBName}/{PIID}/")]
        public HttpResponseMessage GetPINetworks(string DBName, int PIID)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            List<object> formattedRows = new List<object>();


            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Site_Networks
                                where x.Principal_Investigator == PIID
                                select x;

                    foreach (var row in table)
                    {

                        formattedRows.Add(
                            new
                            {
                                Network = row.Network,
                                UniqueIdentifier = row.Unique_Identifier,
                                PrincipalInvestigator = row.Principal_Investigator,
                                Name = row.Name,
                                InstituationName = row.Institution_Name,
                                OriginalFunctinAgency = row.Original_Funding_Agency,
                                GrantNumberString = row.Grant_Number_String,
                                StartedDate = row.Started_Date,
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
