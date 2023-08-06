using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
	public class RepositoryManager : IRepositoryManager
	{
		private readonly RepositoryContext _context; // save işlemini kalıcı hale getirmek için contextimize atmalıyız.(Db)

		private readonly IProductRepository _productRepository;

		private readonly ICategoryRepository _categoryRepository;

		private readonly IOrderRepository _orderRepository;

		public RepositoryManager(IProductRepository productRepository, RepositoryContext context, ICategoryRepository categoryRepository, IOrderRepository orderRepository)
		{
			_productRepository = productRepository;
			_context = context;
			_categoryRepository = categoryRepository;
			_orderRepository = orderRepository;
		}

		public IProductRepository ProductRepository => _productRepository;

		public ICategoryRepository CategoryRepository => _categoryRepository;

		public IOrderRepository OrderRepository => _orderRepository;

		public void Save()
		{
			_context.SaveChanges();
		}
	}
}
