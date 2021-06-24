using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZXing.QrCode;

namespace GuardWebApp.Pages.ScanPage
{
    public class IndexModel : PageModel
    {
        private readonly GuardianDBContext _context;

        public IndexModel(GuardianDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            return Page();
        }

        [BindProperty]
        public string QRVal { get; set; }

        public IActionResult OnPost()
        {
            var uid = HttpContext.Session.GetString("uid");

            if (QRVal != null)
            {
                var location = _context.Locations.Where(a => a.Qr.Equals(QRVal)).FirstOrDefault();
                if(location != null)
                {
                    SubmittedLocation submittedLocation = new SubmittedLocation();
                    submittedLocation.DeviceId = 2;
                    submittedLocation.DateTime = DateTime.Now;
                    submittedLocation.LocationId = location.Id;
                    submittedLocation.UserId = Int64.Parse(uid);

                    _context.SubmittedLocations.Add(submittedLocation);
                    _context.SaveChanges();

                    return RedirectToPage("200");
                }
                else
                {
                    return RedirectToPage("404");
                }
                
            }

            return Page();
        }

    }
}
