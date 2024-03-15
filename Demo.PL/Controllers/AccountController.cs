using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appUser = new ApplicationUser
                {
                    FirstName = model.FName,
                    LastName = model.LName,
                    UserName = model.Email.Split('@')[0],
                    IsAgree = model.IsAgree,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(appUser, model.Password);

                if (result.Succeeded)
                    return RedirectToAction("Login");

                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}
