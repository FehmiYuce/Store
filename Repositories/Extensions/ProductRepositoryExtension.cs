using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Extensions
{
	public static class ProductRepositoryExtension
	{
		public static IQueryable<Product> FilteredByCategoryId(this IQueryable<Product> products, int? categoryId)
		{
			if (categoryId ==null)
			{
				return products;
			}
			else
			{
				return products.Where(p => p.CategoryId.Equals(categoryId));
			}
		}
		public static IQueryable<Product> FilteredBySearchTerm(this IQueryable<Product> products, String? searchTerm)
		{
			if(string.IsNullOrWhiteSpace(searchTerm))
			{
				return products;
			}
			else
			{
				return products.Where(p => p.ProductName.ToLower().Contains(searchTerm.ToLower()));
			}
		}
		public static IQueryable<Product> FilteredByPrice(this IQueryable<Product> products, int maxPrice, int minPrice, bool isValidPrice)
		{
			if(isValidPrice)
			{
				return products.Where(p => p.Price>=minPrice && p.Price<=maxPrice);
			}
			else
			{
				return products;
				//minValue > maxValue' dan büyükken tüm ürünleri getir
			}
			//genişlettiğimiz metod IQueryable<Product> _context üzerinden geliyor
		}
		public static IQueryable<Product> ToPaginate(this IQueryable<Product> products, int pageNumber, int pageSize) 
		{
			return products.Skip((pageNumber - 1) * pageSize).Take(pageSize);
			//1. sayfa 0. elemana denk geliyor.
		}
	}
}
