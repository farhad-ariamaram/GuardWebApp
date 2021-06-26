using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace GuardWebApp.Models
{
    public class ShiftAllocationMD
    {
        [Required(ErrorMessage = "این فیلد اجباری است")]
        public DateTime DateTime { get; set; }
    }

    [ModelMetadataType(typeof(ShiftAllocationMD))]
    public partial class ShiftAllocation
    {
    }
}
