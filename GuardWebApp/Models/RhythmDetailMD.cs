using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace GuardWebApp.Models
{
    public class RhythmDetailMD
    {
        [Required(ErrorMessage = "این فیلد اجباری است")]
        [Range(1, long.MaxValue, ErrorMessage = "عدد وارد شده صحیح نیست")]
        public long OrderNo { get; set; }

        [Required(ErrorMessage = "این فیلد اجباری است")]
        public TimeSpan Time { get; set; }
    }

    [ModelMetadataType(typeof(RhythmDetailMD))]
    public partial class RhythmDetail
    {
    }
}
