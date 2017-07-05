using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NRDC_QC_SPA.Models
{
    public class FlagViewModel
    {
        public byte? QCLV1 { get; set; }
        public byte? QCLV2 { get; set; }
        public int DataStreamId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Deployment { get; set; }
        public string System { get; set; }
        public string Site { get; set; }
        public string SiteAlias { get; set; }
        public string FlagName { get; set; }

    }
}