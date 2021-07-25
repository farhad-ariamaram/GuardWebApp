using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace GuardWebApp.Models
{
    public class VisittimeMD
    {
        [Required(ErrorMessage = "این فیلد اجباری است")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "این فیلد اجباری است")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "این فیلد اجباری است")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "طول این فیلد حداقل 2 و حداکثر 500 کاراکتر می‌باشد")]
        public string Description { get; set; }

        [Required(ErrorMessage = "این فیلد اجباری است")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "طول این فیلد حداقل 2 و حداکثر 50 کاراکتر می‌باشد")]
        public string Title { get; set; }
    }

    [ModelMetadataType(typeof(VisittimeMD))]
    public partial class Visittime
    {
    }
}
