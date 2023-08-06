using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
	public interface IRepositoryBase<T>
	{
		IQueryable<T> FindAll(bool trackChanges); //listeleme yapmak için sorgulanabilir bir ifade tanımladık.

		T? FindByCondition(Expression<Func<T,bool>> expression,bool trackChanges);
		//Bir expression tanımladık, generic bir yapı ve true ya da false olacak.
		void Create(T Entity);
		void Remove(T Entity);
		void Update(T Entity);
	}
		
	
}
