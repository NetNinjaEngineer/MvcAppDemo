using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[AllowAnonymous]
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
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
				ApplicationUser appUser = new ApplicationUser
				{
					FirstName = model.FName,
					LastName = model.LName,
					UserName = model.Email.Split('@')[0],
					IsAgree = model.IsAgree,
					Email = model.Email
				};

				IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);

				if (result.Succeeded)
					return RedirectToAction(nameof(Login));

				else
					foreach (IdentityError error in result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);
			}

			return View(model);
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				ApplicationUser appUser = await _userManager.FindByEmailAsync(model.Email);
				if (appUser is not null)
				{
					bool isPasswordCorrect = await _userManager.CheckPasswordAsync(appUser, model.Password);
					if (isPasswordCorrect)
					{
						// Login
						var loginResult = await _signInManager.PasswordSignInAsync(appUser, model.Password, model.RememberMe, false);
						if (loginResult.Succeeded)
							return RedirectToAction("Index", "Home");
					}
				}
				else
					ModelState.AddModelError(string.Empty, "Invalid Credentials.");

			}

			return View(model);

		}


		public new async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}

		public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var applicationUser = await _userManager.FindByEmailAsync(model.Email);
				if (applicationUser is not null)
				{
					// send email
					var email = new Email
					{
						Subject = "Reset Password",
						Body = "",
						To = model.Email
					};
				}
				else
					ModelState.AddModelError(string.Empty, "Email is not exists");
			}

			return View(nameof(ForgetPassword), model);

		}
	}
}
