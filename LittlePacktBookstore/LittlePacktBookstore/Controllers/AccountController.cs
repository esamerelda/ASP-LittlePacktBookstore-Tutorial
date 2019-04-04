using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittlePacktBookstore.Models;
using LittlePacktBookstore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LittlePacktBookstore.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<SiteUser> _userManager;
		private readonly ILogger<AccountController> _logger;
		private readonly SignInManager<SiteUser> _signinManager;

		public AccountController(UserManager<SiteUser> userManager, ILogger<AccountController> logger, SignInManager<SiteUser> signinManager)
		{
			_userManager = userManager;
			_logger = logger;
			_signinManager = signinManager;
		}

        public IActionResult Index()
        {
            return View();
        }

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegistrationViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new SiteUser
				{
					UserName = model.UserName,
					Email = model.UserName,
					FirstName = model.FirstName,
					LastName = model.LastName
				};
				var result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}
				else
				{
					foreach(var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}
			return View();
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await _signinManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
				if (result.Succeeded)
				{
					if (Request.Query.Keys.Contains("ReturnUrl"))
					{
						return Redirect(Request.Query["ReturnUrl"].FirstOrDefault());
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}
				else
				{
					ModelState.AddModelError("", "Failed to Log In.");
				}
			}
			return View();
		}

		public async Task<IActionResult> Logout()
		{
			if (User.Identity.IsAuthenticated)
			{
				await _signinManager.SignOutAsync();
			}
			return RedirectToAction("Index", "Home");
		}
	}

}