using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GuardWebApp.Controllers;
using GuardWebApp.Controllers.Utils;
using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuardWebApp.Pages.GuardPage
{
    public class CreateModel : PageModel
    {
        private readonly GuardianDBContext _db;

        public CreateModel(GuardianDBContext db)
        {
            _db = db;
        }

        [BindProperty]
        public userViewModel User { get; set; }

        public IActionResult OnGet()
        {
            var uid = HttpContext.Session.GetString("uid");
            if (uid == null)
            {
                return RedirectToPage("../Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongUP", "نام کاربری یا کلمه عبور اشتباه است");
                return Page();
            }

            UsersController api = new UsersController(_db);
            await api.Login(User.Username, User.Password);

            var checkUser = _db.Users.Where(a => a.Username == User.Username && a.Password == ApiUtilities.sha512(User.Password + ApiUtilities._SALT)).FirstOrDefault();
            if (checkUser != null)
            {
                ViewData["userAdded"] = true;
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError("WrongUP", "نام کاربری یا کلمه عبور اشتباه است");
            return Page();

        }
    }

    public class userViewModel
    {
        [Required(ErrorMessage = "نام کاربری اجباری است")]
        public string Username { get; set; }

        [Required(ErrorMessage = "رمز ورود اجباری است")]
        public string Password { get; set; }

    }
}
