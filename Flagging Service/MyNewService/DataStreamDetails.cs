using NRDC.Projects.Models.GIDMIS;
using NRDC_QC_Flagging_Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FlaggingService
{
    //create get and sets for the data stream from the SQL server
    // used to make flag decisions
    public class DataStreamDetails
    {
        public Components Component { get; set; }
        public int Deployment { get; set; }
        public int DataStream { get; set; }
        public int MeasurementNumber { get; set; }
        public List <Measurement> Measurements { get; set; }
    }

    public class DataStream
    {

        public DataStream()
        {
            DataStreamDetails = new List<DataStreamDetails>();
        }

        private bool Fill(IsRunning run, EventLog eventLog1)
        {
            try
            {
                // connect to the SQL server
                using (var db = new GIDMISContainer(HelperFunctions.getConnectionString()))
                {
                    try
                    {
                        // check if datastreams have been added or removed
                        if (DataStreamDetails.Count != db.Data_Streams.Count())
                        {
                            // select all column from the datastream database
                            var DataStreamTable = from x in db.Data_Streams
                                                  select x;

                            // get info from the datastream
                            foreach (var row in DataStreamTable)
                            {
                                DataStreamDetails.Add(
                                new DataStreamDetails
                                {
                                    Deployment = row.Deployment,
                                    DataStream = row.Stream
                                });
                            }
                        }

                        // cycle through datastreams
                        for (int i = 0; i < DataStreamDetails.Count; i++)
                        {
                            DataStreamDetails[i].Component = CollectDetails(DataStreamDetails[i].Deployment, db, run, eventLog1);

                            if (DataStreamDetails[i].Component == null)
                            {
                                // throw exception
                                throw new Exception("No Component Related to " + DataStreamDetails[i].Deployment + "found");
                            }

                            //create measurement variables
                            if (DataStreamDetails[i].Measurements == null || DataStreamDetails[i].Measurements.Count > 1000)
                            {
                                DataStreamDetails[i].Measurements = null;
                                DataStreamDetails[i].Measurements = new List<Measurement>();
                            }

                            int size = DataStreamDetails[i].Measurements.Count;

                            // grab only the measurements not yet taken. only looks at the top 10000
                            var MeasurementTable = (from x in db.Measurements
                                                    where x.Stream == DataStreamDetails[i].DataStream
                                                    where x.Measurement_Time_Stamp > DataStreamDetails[i].Measurements.AsQueryable().Last().Measurement_Time_Stamp
                                                    orderby x.Measurement_Time_Stamp ascending
                                                    select x).Take(1000);

                            // put the values in the list
                            foreach (var row in MeasurementTable.Skip(size))
                            {
                                Flag(row, DataStreamDetails[i].Component);
                               
                            }

                            db.SaveChanges();
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        ErrorHandling.ExceptionHandler(e, run, eventLog1);
                        return false;
                    }
                }
            } catch (Exception e)
            {
                ErrorHandling.ExceptionHandler(e, run, eventLog1);
                return false;
            }
        }

        private void Flag(Measurements row, Components component)
        {
            // Flag 1
            // if the flag is above or below the component specifications
            if (row.Value >= component.Maximum
               || row.Value <= component.Minimum)
            {
                if (!component.UserSpecified)
                {
                    row.L1_Quality_Control = new L1_Quality_Control
                    {
                        Name = "Out of Bounds",
                        Description = "The measurment value has gone either abover or below the component's specified bounds."
                    };
                }

                else
                {
                    row.L2_Quality_Control = new L2_Quality_Control
                    {
                        Name = "Out of Bounds",
                        Description = "The measurment value has gone either abover or below the component's specified bounds."
                    };
                }

            }
        }

        private Components CollectDetails(int deployment, GIDMISContainer db, IsRunning run, EventLog eventLog1)
        {
            try
            {
                // find most recent component associated with the deployment
                var RelatedComponent = (from x in db.Components
                                        where x.Deployment == deployment
                                        orderby x.Installation_Date descending
                                        select x).First();

                var ComponentSpecification = (from x in db.Component_Specifications
                                              where x.Component == RelatedComponent.Component
                                              select x).First();

                return new Components
                {
                    ComponentNumber = RelatedComponent.Component,
                    InstallationDate = RelatedComponent.Installation_Date,

                    Maximum = ComponentSpecification.Maximum,
                    Minimum = ComponentSpecification.Minimum,
                    UserSpecified = ComponentSpecification.User_Specified
                };

            } catch (Exception e)
            {
                ErrorHandling.ExceptionHandler(e, run, eventLog1);
                return null;
            }
        }


        // create list for datastream
        public List<DataStreamDetails> DataStreamDetails = null;
    }

}
