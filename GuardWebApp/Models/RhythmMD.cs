using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace GuardWebApp.Models
{
    public class RhythmMD
    {
        [Required(ErrorMessage = "این فیلد اجباری است")]
        public DateTime DateTime { get; set; }
    }

    [ModelMetadataType(typeof(RhythmMD))]
    public partial class Rhythm
    {
    }
}
