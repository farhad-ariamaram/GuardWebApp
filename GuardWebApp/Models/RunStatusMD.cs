using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GuardWebApp.Models
{
    public class RunStatusMD
    {
        [Required(ErrorMessage = "این فیلد اجباری است")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "طول این فیلد حداقل 2 و حداکثر 50 کاراکتر می‌باشد")]
        public string Name { get; set; }
    }

    [ModelMetadataType(typeof(RunStatusMD))]
    public partial class RunStatus
    {
    }
}
