using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Data;

namespace StoreApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class OrderController : Controller
	{
		private readonly IServiceManager _serviceManager;

		public OrderController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}
		public IActionResult Index()
		{
			var orders = _serviceManager.OrderService.Orders;
			ViewData["Title"] = "Orders";
			return View(orders);
		}
		[HttpPost]
		public IActionResult Complete([FromForm]int id)
		{
			_serviceManager.OrderService.Complete(id);
			return RedirectToAction("Index");
		}
	}
}
