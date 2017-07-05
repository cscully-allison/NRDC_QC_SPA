namespace FlaggingService
{
    //create get and sets for the measurement from the SQL server
    //used to set flag decisions
   public class Measurement
    {
        public decimal? Value { get; set; }
        public Flags Flag1 { get; set; }
        public Flags Flag2 { get; set; }
        public System.DateTime Measurement_Time_Stamp { get; set; }

    }
}
