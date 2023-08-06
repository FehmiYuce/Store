using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StoreApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles ="Admin")]
	public class CategoryController : Controller
	{
		
		public IActionResult Index()
		{
			ViewData["Title"] = "Categories";
			return View();
		}
	}
}
