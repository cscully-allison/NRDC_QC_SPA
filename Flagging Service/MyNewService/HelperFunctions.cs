using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRDC_QC_Flagging_Service
{
    public class HelperFunctions
    {

        public static string getConnectionString()
        {
            return String.Format(ConfigurationManager.ConnectionStrings["GIDMISContainer"].ConnectionString);
        }

        public static void WriteToFile(string text)
        {
            using (StreamWriter writer = new StreamWriter("C:\\Log-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", true))
            {
                writer.WriteLine(string.Format(text, DateTime.Now.ToString("hh:mm:ss tt")));
                writer.Close();
            }
        }

    }

    public class ErrorHandling
    {
        public static void ExceptionHandler(Exception e, IsRunning run, EventLog eventLog1)
        {
            run.setFalse();
            // record the error
            HelperFunctions.WriteToFile("Error: " + e.Message + e.InnerException);
            eventLog1.WriteEntry("Error: " + e.Message + e.StackTrace);
            // if this stays in you get like 10 emails when something goes wrong
            // Report(e.Message + "<br />" + e.InnerException);
        }


    }

    public class IsRunning
    {
        public IsRunning()
        {
            if (!running.HasValue)
            {
                running = true;
            }
        }

        public void setFalse()
        {
            running = false;
        }

        public void setTrue()
        {
            running = true;
        }

        public bool check()
        {
            return (bool) running;
        }

        private bool? running;
    }
}
