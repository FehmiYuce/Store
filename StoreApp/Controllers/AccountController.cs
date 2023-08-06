using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Models;
using System.Reflection.Metadata.Ecma335;

namespace StoreApp.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Login([FromQuery(Name = "ReturnUrl")] string ReturnUrl = "/")
		{
			ViewData["Title"] = "Login";
			return View(new LoginModel()
			{
				ReturnUrl= ReturnUrl
			});
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login([FromForm]LoginModel model)
		{
			if (ModelState.IsValid)
			{
				IdentityUser user = await _userManager.FindByNameAsync(model.Name);
				if(user is not null)
				{
					//Oturum açma 
					await _signInManager.SignOutAsync(); //oturum açmadan önce başka açık oturum varsa kapatır
					if ((await _signInManager.PasswordSignInAsync(user, model.Password, false, false)).Succeeded)
					{
						return Redirect(model?.ReturnUrl ?? "/");
					}
				}//kullanıcının tanınması
				else
				{
					ModelState.AddModelError("Error", "Invalid user name or password.");
				}
				
			}//form doğru ise(giriş bilgileri)
			return View(model);
		}
		public async Task<IActionResult> Logout([FromQuery(Name ="ReturnUrl")] string ReturnUrl = "/")
		{
			await _signInManager.SignOutAsync();
			return Redirect(ReturnUrl);
		}
		public IActionResult Register()
		{
            ViewData["Title"] = "Register";
            return View();
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm]RegisterDto registerDto)
		{
			var user = new IdentityUser()
			{
				UserName = registerDto.UserName,
				Email = registerDto.Email,
			};//kullanıcı oluştu
			var result = await _userManager.CreateAsync(user, registerDto.Password); //kaydettik
			if(result.Succeeded)
			{
				var roleResult = await _userManager.AddToRoleAsync(user,"User");//(kullanıcı, rol tanımı),user rolü atadık
				if(roleResult.Succeeded)
				{
					return RedirectToAction("Login", new {ReturnUrl="/"}); //returnUrl'ye gerek yok, kaydolup giriş yaptıktan sonra kullanıcı ana sayfaya gitsin
				}
            }//eğer başarıyla eklendiyse rol ekledik 
            else
			{
				foreach (var error in result.Errors)//IdentityResult tan geliyor
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			
			return View();
		}
		public IActionResult AccessDenied([FromQuery(Name = "ReturnUrl")]string returnUrl)
		{
			//Bir QueryString yapısı oluşturabiliriz.
			return View();
		}
	}
}
