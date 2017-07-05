using System;

namespace FlaggingService
{
    //create get and sets for the components from the SQL server
    // used to make flag decisions
    public class Components
    {
        public int ComponentNumber { get; set; }
        public DateTime InstallationDate { get; set; }
        public bool UserSpecified { get; set; }
       // public int Unit { get; set; }
        public decimal Minimum { get; set; }
        public decimal Maximum { get; set; }
    }
}
