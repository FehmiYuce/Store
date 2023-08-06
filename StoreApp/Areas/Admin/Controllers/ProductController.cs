using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Contracts;
using StoreApp.Models;
using System.Data;

namespace StoreApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ProductController : Controller
	{
		private readonly IServiceManager _serviceManager;

		public ProductController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}

		public IActionResult Index([FromQuery]ProductRequestParameters p)
		{
			ViewData["Title"] = "Products";
			var products = _serviceManager.ProductService.GetAllProductsWithDetails(p);
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
		public IActionResult Create()
		{
			TempData["info"] = "Please fill the form.";
			ViewBag.Categories = GetCategoriesSelectList();
			return View();
		}
		private SelectList GetCategoriesSelectList()
		{
			return new SelectList(_serviceManager.CategoryService.GetAllCategories(false), "CategoryId", "CategoryName", "1");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([FromForm]ProductDtoForInsertion productDto, IFormFile file)
		{
			if(ModelState.IsValid)
			{
				// file operation
				string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images",file.FileName);
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}
				//maliyetli işler için bunu kullanırız. parantezin içindekiler kullanıldıktan sonra programın geri kalanı tarafından kullanılmaz, parantezden çıkılır
				productDto.ImageUrl = String.Concat("/images/",file.FileName);
				_serviceManager.ProductService.CreateProduct(productDto);
				TempData["success"] = $"{productDto.ProductName} has been created.";
				return RedirectToAction("Index");
			}
			return View();
		}
		public IActionResult Update([FromRoute(Name ="id")]int id)
		{
			ViewBag.Categories = GetCategoriesSelectList();
			var model = _serviceManager.ProductService.GetOneProductForUpdate(id, false);
			ViewData["Title"] = model?.ProductName;
			return View(model);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update([FromForm]ProductDtoForUpdate productDto, IFormFile file)
		{
			if(ModelState.IsValid)
			{
				string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", file.FileName);
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}
				productDto.ImageUrl = String.Concat("/images/", file.FileName);
				_serviceManager.ProductService.UpdateOneProduct(productDto);
				return	RedirectToAction("Index");
			}
			return View();
		}
		[HttpGet]
		public IActionResult Delete([FromRoute(Name ="id")]int id)
		{
			_serviceManager.ProductService.DeleteOneProduct(id);
			TempData["danger"] = "The product has been removed.";
			return RedirectToAction("Index");
		}
	}
}
