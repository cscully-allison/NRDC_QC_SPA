﻿using Newtonsoft.Json;
using NRDC.Projects.Models.GIDMIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NRDC_QC.APIs
{
    public class SitesController : ApiController
    {
        // GET: api/sites/{DBName}
        public HttpResponseMessage Get(string DBName)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            List<object> formattedRows = new List<object>();

            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Sites
                                select x;

                    foreach (var row in table)
                    {
                        formattedRows.Add(new
                        {
                            SiteID = row.Site,
                            UniqueIdentifier = row.Unique_Identifier,
                            Network = row.Network,
                            LandOwner = row.Land_Owner,
                            Name = row.Name,
                            Alias = row.Alias,
                            Notes = row.Notes,
                            Location = row.Location,
                            TimeZoneName = row.Time_Zone_Name,
                            TimeZoneAbbrevation = row.Time_Zone_Abbreviation,
                            TimeZoneOffset = row.Time_Zone_Offset,
                            CreationDate = row.Creation_Date,
                            ModificationDate = row.Modification_Date,
                            GPSLandmark = row.GPS_Landmark,
                            Photo = row.Landmark_Photo
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
        public HttpResponseMessage Get(string DBName, int id)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;


            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var row = (from x in db.Sites
                               where x.Site == id
                               select x).FirstOrDefault();
                    var returned = new
                    {
                        SiteID = row.Site,
                        UniqueIdentifier = row.Unique_Identifier,
                        Network = row.Network,
                        LandOwner = row.Land_Owner,
                        Name = row.Name,
                        Alias = row.Alias,
                        Notes = row.Notes,
                        Location = row.Location,
                        TimeZoneName = row.Time_Zone_Name,
                        TimeZoneAbbrevation = row.Time_Zone_Abbreviation,
                        TimeZoneOffset = row.Time_Zone_Offset,
                        CreationDate = row.Creation_Date,
                        ModificationDate = row.Modification_Date,
                        GPSLandmark = row.GPS_Landmark,
                        Photo = row.Landmark_Photo
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


        //get by networkID
        [Route("api/Sites/GetNetworkSites/{DBName}/{NetworkId}/")]
        public HttpResponseMessage GetNetworkSites(string DBName, int NetworkId)
        {
            ConnectionHelper conn = new ConnectionHelper();
            string json;
            List<object> formattedRows = new List<object>();


            try
            {
                using (var db = new GIDMISContainer(conn.getConnectionString(DBName)))
                {
                    var table = from x in db.Sites
                                where x.Network == NetworkId
                                select x;

                    foreach (var row in table)
                    {

                        formattedRows.Add(
                            new
                            {
                                SiteID = row.Site,
                                UniqueIdentifier = row.Unique_Identifier,
                                Network = row.Network,
                                LandOwner = row.Land_Owner,
                                Name = row.Name,
                                Alias = row.Alias,
                                Notes = row.Notes,
                                Location = row.Location,
                                TimeZoneName = row.Time_Zone_Name,
                                TimeZoneAbbrevation = row.Time_Zone_Abbreviation,
                                TimeZoneOffset = row.Time_Zone_Offset,
                                CreationDate = row.Creation_Date,
                                ModificationDate = row.Modification_Date,
                                GPSLandmark = row.GPS_Landmark,
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
