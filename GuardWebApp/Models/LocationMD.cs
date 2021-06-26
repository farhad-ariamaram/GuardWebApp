using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace GuardWebApp.Models
{
    public class LocationMD
    {
        [Required(ErrorMessage = "این فیلد اجباری است")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "طول این فیلد حداقل 2 و حداکثر 50 کاراکتر می‌باشد")]
        public string Name { get; set; }

        [Required(ErrorMessage = "این فیلد اجباری است")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "طول این فیلد حداقل 2 و حداکثر 1000 کاراکتر می‌باشد")]
        public string Qr { get; set; }

        [Required(ErrorMessage = "این فیلد اجباری است")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "طول این فیلد حداقل 2 و حداکثر 1000 کاراکتر می‌باشد")]
        public string Nfc { get; set; }
    }

    [ModelMetadataType(typeof(LocationMD))]
    public partial class Location
    {
    }
}
