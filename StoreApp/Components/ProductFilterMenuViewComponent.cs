using Microsoft.AspNetCore.Mvc;

namespace StoreApp.Components
{
	public class ProductFilterMenuViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
			//invoke = çağır
			//Partial view ile de halledebilirdik lakin filter ya da veri tabanı ile çalışmak istersek ViewComponent yapısı daha uygun olur.
		}
	}
}
