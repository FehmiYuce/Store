using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.Contracts;
using System.Data;

namespace StoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")] //Admin sayfasına yetkisi olmayanların geçemeyeceğini söyledik. Kullanıcıları rollere göre kısıtlar
    //Çünkü bir user giriş yaptığında admini görmüyor ama endpointlerle(url' sini girerek) admin paneline ulaşıyordu
	public class UserController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public UserController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public IActionResult Index()
        {
            var model = _serviceManager.AuthService.GetAllUsers();
            ViewData["Title"] = "Users";  
            return View(model);
        }
        public IActionResult Create() 
        {
            return View(new UserDtoForCreation()
            {
                Roles = new HashSet<string>(
                    _serviceManager
                    .AuthService
                    .Roles //IdentityRole yapısında
                    .Select(r => r.Name) //sadece name' lerini alarak role yapısından çıkardık.
					.ToList()) //List<String> dönüyoruz artık.
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserDtoForCreation userDto)
        {
            var result = await _serviceManager.AuthService.CreateUser(userDto);
            return result.Succeeded
                ? RedirectToAction("Index") 
                : View();
        }
        public async Task<IActionResult> Update([FromRoute(Name ="id")]string id)
        {
            var user = await _serviceManager.AuthService.GetOneUserForUpdate(id); //id aslında userName, index sayfasında öyle belirledik.
            ViewData["Title"] = user?.UserName;
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm]UserDtoForUpdate userDto)
        {
            if (ModelState.IsValid)
            {
                await _serviceManager.AuthService.UpdateUser(userDto);
                return RedirectToAction("Index");
			}
            return View();
        }
        public async Task<IActionResult> ResetPassword([FromRoute(Name ="id")]string id)
        {
            return View(new ResetPasswordDto()
            {
                UserName = id,
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword([FromForm]ResetPasswordDto passwordDto)
        {
            var result = await _serviceManager.AuthService.ResetPassword(passwordDto);
            return result.Succeeded 
                ? RedirectToAction("Index") 
                : View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOneUser([FromForm] UserDto userDto)
        {
            var result = await _serviceManager.AuthService.DeleteOneUser(userDto.UserName);
            return result.Succeeded ? RedirectToAction("Index") : View();
        }
    }
}
