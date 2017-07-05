﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FlaggingService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            // attach debugger
            System.Diagnostics.Debugger.Launch();
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new NRDC_QC_Flagging_Service()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
