using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace GuardWebApp.Models
{
    public class ViolationConsequenceMD
    {
        [Required(ErrorMessage = "این فیلد اجباری است")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "طول این فیلد حداقل 2 و حداکثر 500 کاراکتر می‌باشد")]
        public string Description { get; set; }
    }

    [ModelMetadataType(typeof(ViolationConsequenceMD))]
    public partial class ViolationConsequence
    {
    }
}
