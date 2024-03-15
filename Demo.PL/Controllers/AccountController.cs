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
					var token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
					var resetPasswordLink = Url.Action("ResetPassword", "Account", new { email = applicationUser.Email, Token = token }, Request.Scheme);
					var email = new Email
					{
						Subject = "Reset Password",
						Body = resetPasswordLink,
						To = model.Email
					};

					EmailSettings.SendEmail(email);
					return RedirectToAction(nameof(CheckYourInBox));
				}
				else
					ModelState.AddModelError(string.Empty, "Email is not exists");
			}

			return View(nameof(ForgetPassword), model);

		}

		public IActionResult CheckYourInBox()
		{
			return View();
		}

		public IActionResult ResetPassword(string email, string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				string token = TempData["token"] as string;
				string email = TempData["email"] as string;
				var user = await _userManager.FindByEmailAsync(email);
				if (user is not null)
				{
					var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
					if (result.Succeeded)
						return RedirectToAction(nameof(Login));
					else
						foreach (var error in result.Errors)
							ModelState.AddModelError(string.Empty, error.Description);
				}


			}
			return View(model);
		}
	}
}
