using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace GuardWebApp.Models
{
    public class ViolationMD
    {
        [Required(ErrorMessage = "این فیلد اجباری است")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "طول این فیلد حداقل 2 و حداکثر 500 کاراکتر می‌باشد")]
        public string Description { get; set; }

        [Required(ErrorMessage = "این فیلد اجباری است")]
        public DateTime ApproveDate { get; set; }

        [Required(ErrorMessage = "این فیلد اجباری است")]
        public DateTime RegisterDate { get; set; }
    }

    [ModelMetadataType(typeof(ViolationMD))]
    public partial class Violation
    {
    }
}
