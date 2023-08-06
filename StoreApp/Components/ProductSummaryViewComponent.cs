using Microsoft.AspNetCore.Mvc;
using Repositories;
using Services;
using Services.Contracts;

namespace StoreApp.Components
{
	public class ProductSummaryViewComponent : ViewComponent
	{
		private readonly IServiceManager _serviceManager;

		public ProductSummaryViewComponent(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}

		public String Invoke()
		{
			return _serviceManager.ProductService.GetAllProducts(false).Count().ToString();
		}
		//layout' daki product' ın yanındaki sayı (ürün sayısını bulmamız için).
	}
}
