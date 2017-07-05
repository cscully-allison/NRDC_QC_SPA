using System;

namespace FlaggingService
{
    //create get and sets for the flags from the SQL server
    // used to set flag decisions
    public class Flags
    {
        public bool Flag { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

}
