using Entities.Models;
using Entities.RequestParameters;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;
using StoreApp.Models;

namespace StoreApp.Controllers
{
	public class ProductController : Controller
	{
		/*private readonly IRepositoryManager _repositoryManager;*/ //IRepositoryManager ın için de IProductManager de IRepositoryContext de var

		//public ProductController(IRepositoryManager repositoryManager)
		//{
		//	_repositoryManager = repositoryManager;
		//}
		private readonly IServiceManager _serviceManager;

		public ProductController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}

		public IActionResult Index(ProductRequestParameters p)
		{
			var products = _serviceManager.ProductService.GetAllProductsWithDetails(p);
			ViewData["Title"] = "Products";
			var pagination = new Pagination() //sayfalara belirli ürün sayısını burda getiriyoruz
			{
				CurrentPage = p.PageNumber,
				ItemsPerPage = p.PageSize,
				TotalItems = _serviceManager.ProductService.GetAllProducts(false).Count()
				// pagination parametreleri = entity ve veritabanından gelen veriler
			};
			return View(new ProductListViewModel()
			{
				Products = products,
				Pagination = pagination
			});
		}

		public IActionResult Get([FromRoute(Name = "id")]int id)
		{
			var model = _serviceManager.ProductService.GetOneProduct(id,false);
			ViewData["Title"] = model?.ProductName;
			return View(model);
		}
	}
}
