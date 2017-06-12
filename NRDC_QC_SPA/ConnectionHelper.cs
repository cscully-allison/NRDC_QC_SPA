using System;
using System.Configuration;
using System.Net.Http;

namespace NRDC_QC.APIs
{
    public class ConnectionHelper
    {
        //Members
        public string storedConnString { get; set; }
        public string SQLConnString { get; set; }
        public string EntityClientConnString { get; set; }

        public string getConnectionString(string DBName)
        {        
            //get a sql connection string matching DBName
            return String.Format(ConfigurationManager.ConnectionStrings["GIDMISContainer"].ConnectionString, DBName);
        }

        public HttpResponseMessage BuildJsonResponse(string JSON)
        {
            HttpResponseMessage Response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            Response.Content = new StringContent(JSON, System.Text.Encoding.UTF8, "application/json");
            return Response;
        }


    }
}