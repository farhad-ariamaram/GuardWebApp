using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GuardWebApp.Models
{
    public class GuardAreaAllocationMD
    {
        [Required(ErrorMessage = "این فیلد اجباری است")]
        public DateTime StartDate { get; set; }
    }

    [ModelMetadataType(typeof(GuardAreaAllocationMD))]
    public partial class GuardAreaAllocation
    {
    }
}
