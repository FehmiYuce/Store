using Entities.Models;
using Entities.RequestParameters;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
	//sealed bir class artık kalıtılamaz
	public sealed class ProductRepository : RepositoryBase<Product>, IProductRepository
	{
		public ProductRepository(RepositoryContext context) : base(context) //contexti base classa gönderiyoruz
		{
		}

		public void CreateOneProduct(Product product) => Create(product);

		public void DeleteOneProduct(Product product) => Remove(product);

		public IQueryable<Product> GetAll(bool trackChanges) => FindAll(trackChanges);

		public IQueryable<Product> GetAllProductsWithDetails(ProductRequestParameters parameters)
		{
			//return parameters.CategoryId is null
			//	? _context.Products.Include(p => p.Category)//eğer categoryId boşsa yani herhangi bir filtreleme yapılmamışsa tüm ürünleri getir
			//	: _context.Products.Include(p => p.Category).Where(p => p.CategoryId.Equals(parameters.CategoryId));
			////ürünün kategori id' si ile kategori id eşit olduğunda filtrele 
			return _context
				.Products
				.FilteredByCategoryId(parameters.CategoryId)
				.FilteredBySearchTerm(parameters.SearchTerm)
				.FilteredByPrice(parameters.MaxPrice, parameters.MinPrice, parameters.IsValidPrice)
				.ToPaginate(parameters.PageNumber, parameters.PageSize);
		}

		public Product? GetOneProduct(int id, bool trackChanges)
		{
			return FindByCondition(p => p.ProductId.Equals(id), trackChanges);
		}

		public IQueryable<Product> GetShowcaseProducts(bool trackChanges)
		{
			return FindAll(trackChanges).Where(p => p.ShowCase.Equals(true));

		}

		public void UpdateOneProduct(Product Entity) => Update(Entity);
		

		// => <-> return

	}
}
