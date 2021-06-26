using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

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
