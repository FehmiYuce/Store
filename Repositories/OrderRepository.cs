using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
	public class OrderRepository : RepositoryBase<Order>, IOrderRepository
	{
		public OrderRepository(RepositoryContext context) : base(context)
		{
		}

		public IQueryable<Order> Orders => _context.Orders.
			Include(o => o.Lines). //her bir sipariş
			ThenInclude(cl => cl.Product). //yanında ürünleri de getir
			OrderBy(s=> s.Shipped). // teslim edilme durumuna göre sırala
			ThenByDescending(o => o.OrderId); // ardından orderId' ye göre sırala

		public int NumberOfInProcess => _context.Orders.Count(o => o.Shipped.Equals(false));
		//teslim edilmeyenlerin sayısını al

		public void Complete(int id)
		{
			var order = FindByCondition(o => o.OrderId.Equals(id), true);
			if(order is null)
			{
				throw new Exception("Order is not found");
			}
			order.Shipped = true;
		}


		public Order? GetOneOrder(int id)
		{
			return FindByCondition(o => o.OrderId.Equals(id), false);
		}
		

		public void SaveOrder(Order order)
		{
			_context.AttachRange(order.Lines.Select(o => o.Product));
			// birden fazla order gelebilir. Line'daki productları order'a ekler
			if (order.OrderId==0)
			{
				_context.Orders.Add(order);
			}
			//orderId' si yoksa ürünü ekler
			_context.SaveChanges();
		}
	}
}