using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
	public interface IRepositoryManager 
	{
		IProductRepository ProductRepository { get; }
		ICategoryRepository CategoryRepository { get; }
		IOrderRepository OrderRepository { get; }
		void Save();
	}
}
