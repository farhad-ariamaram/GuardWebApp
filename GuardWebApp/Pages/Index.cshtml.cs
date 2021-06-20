using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GuardWebApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public userViewModel User { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("./Panel");
        }
    }

    public class userViewModel
    {
        [Required(ErrorMessage ="نام کاربری اجباری است")]
        public string Username { get; set; }

        [Required(ErrorMessage = "رمز ورود اجباری است")]
        public string Password { get; set; }

    }
}
