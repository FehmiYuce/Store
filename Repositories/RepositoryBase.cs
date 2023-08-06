using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
	public abstract class RepositoryBase<T> : IRepositoryBase<T>
		where T : class, new()
	{
		protected readonly RepositoryContext _context;
		//veri ile ilgili işlem yapacağımız için contextimizi atadık ve constructor ını oluşturduk.

		protected RepositoryBase(RepositoryContext context)
		{
			_context = context;
		}

		public void Create(T Entity)
		{
			_context.Set<T>().Add(Entity);
		}

		public IQueryable<T> FindAll(bool trackChanges)
		{
			return trackChanges
				? _context.Set<T>()  // true ise yerleşcecz  . efcore iki durumdada izleyecek.
				: _context.Set<T>().AsNoTracking(); // false ise asNoTracking ile yerleşmeye çalışacağız.
		}

		public T? FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
		{
			return trackChanges
				? _context.Set<T>().Where(expression).SingleOrDefault()
				: _context.Set<T>().Where(expression).AsNoTracking().SingleOrDefault();
		}

		public void Remove(T Entity)
		{
			_context.Set<T>().Remove(Entity);
		}

		public void Update(T Entity)
		{
			_context.Set<T>().Update(Entity);
		}
	}
	// abstract dememizin sebebi, temel(base) class new' lenemesin diye. interface new' lenemez. Repositoryi miras alan
	// category, product vs vs. new' lenebilecek.
}
