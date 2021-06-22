using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GuardWebApp.Models
{
    public class ShiftMD
    {
        [Required(ErrorMessage = "این فیلد اجباری است")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "طول این فیلد حداقل 2 و حداکثر 50 کاراکتر می‌باشد")]
        public string Title { get; set; }
    }

    [ModelMetadataType(typeof(ShiftMD))]
    public partial class Shift
    {
    }
}
