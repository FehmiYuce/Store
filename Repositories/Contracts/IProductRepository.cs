using Entities.Models;
using Entities.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
	public interface IProductRepository : IRepositoryBase<Product>
	{
		IQueryable<Product> GetAll(bool trackChanges);
		IQueryable<Product> GetShowcaseProducts(bool trackChanges);
		IQueryable<Product> GetAllProductsWithDetails(ProductRequestParameters parameters);
		Product? GetOneProduct(int id,bool trackChanges);
		void CreateOneProduct(Product product);
		void DeleteOneProduct(Product product);
		void UpdateOneProduct(Product Entity);
	}
}
