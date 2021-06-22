using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GuardWebApp.Models
{
    public class PlanMD
    {
        [Required(ErrorMessage = "این فیلد اجباری است")]
        public DateTime DateTime { get; set; }
    }

    [ModelMetadataType(typeof(PlanMD))]
    public partial class Plan
    {
    }
}
