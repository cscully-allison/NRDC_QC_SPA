using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NRDC_QC_SPA.Models
{
    // Models returned by MeController actions.
    public class GetViewModel
    {
        public string Hometown { get; set; }
    }
}