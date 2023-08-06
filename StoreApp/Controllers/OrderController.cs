using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Controllers
{
	public class OrderController : Controller
	{
		private readonly IServiceManager _serviceManager;
		private readonly Cart _cart;

		public OrderController(IServiceManager serviceManager, Cart cart)
		{
			_serviceManager = serviceManager;
			_cart = cart;
		}

		[Authorize] //kullanıcı login olamadan sipariş veremesin.
		public ViewResult CheckOut()
		{
			ViewData["Title"] = "Checkout";
			return View("Index",new Order());
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Checkout([FromForm]Order order)
		{
			if (_cart.Lines.Count()==0)
			{
				ModelState.AddModelError("", "Sorry, your cart is empty.");
			}
			if(ModelState.IsValid)
			{
				order.Lines = _cart.Lines.ToArray(); //cart bilgilerini order' ın üzerine taşıyoruz
				_serviceManager.OrderService.SaveOrder(order);
				_cart.Clear(); //order edildikten sonra cart bilgisini temizler.(hem session hem de ürünler temizleniyor)
				return RedirectToPage("/Complete", new { OrderId = order.OrderId });
			}
			else
			{
				return View(); //Model geçersiz olursa mevcut sayfaya geri dönsün
			}
			
		}
	}
}
