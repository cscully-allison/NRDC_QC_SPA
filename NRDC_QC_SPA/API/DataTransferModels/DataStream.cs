using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NRDC.Projects.Models.GIDMIS;

namespace NRDC_QC.APIs.DataTransferModels
{
    public class DataStream
    {
        public DataStream()
        {

        }

        public DataStream(Data_Streams dataStreamRow)
        {

            DataStreamID = dataStreamRow.Stream;
            Category = dataStreamRow.Categories.Name;
            CategoryID = dataStreamRow.Category;
            Deployment = dataStreamRow.Deployments.Name;
            DeploymentID = dataStreamRow.Deployment;
            Property = dataStreamRow.Properties.Name;
            PropertyID = dataStreamRow.Property;
            DataType = dataStreamRow.Data_Types.Name;
            TypeID = dataStreamRow.Type;
            Unit = dataStreamRow.Units.Name;
            UnitID = dataStreamRow.Unit;
            Started = dataStreamRow.Started.ToShortDateString() + " " + dataStreamRow.Started.ToLongTimeString();
            Ended = Convert.ToDateTime(dataStreamRow.Ended).ToShortDateString() + " " + Convert.ToDateTime(dataStreamRow.Ended).ToLongTimeString();
            DataInterval = dataStreamRow.Data_Interval;
        }

        public int DataStreamID { get; set; }
        public string Deployment { get; set; }
        public int DeploymentID { get; set; }
        public string Category { get; set; }
        public int CategoryID { get; set; }
        public string Property { get; set; }
        public int PropertyID { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public string DataType { get; set; }
        public int TypeID { get; set; }
        public TimeSpan DataInterval { get; set; }
        public string Started { get; set; }
        public string Ended { get; set; }
    }
}