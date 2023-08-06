using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Components
{
	public class ShowcaseViewComponent : ViewComponent
	{
		private readonly IServiceManager _serviceManager;

		public ShowcaseViewComponent(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}
		public IViewComponentResult Invoke(string page = "default")
		{
			var products = _serviceManager.ProductService.GetShowcaseProducts(false);
			return page.Equals("default")
				? View(products) //varsayılan şekilde çalış. default.cshtml(Normal kullanıcı araüyüzünde gözüküyor)
				: View("List",products); //ama eşit değilse List' in içindeki productları getir.List.cshml(Admin arayüzünde gözüküyor)
        }
	}
}
