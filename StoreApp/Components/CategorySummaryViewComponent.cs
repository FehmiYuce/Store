using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Components
{
	public class CategorySummaryViewComponent : ViewComponent
	{
		private readonly IServiceManager _serviceManager;

		public CategorySummaryViewComponent(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}
		public string Invoke()
		{
			return _serviceManager.CategoryService.GetAllCategories(false).Count().ToString();
		}
	}
}
