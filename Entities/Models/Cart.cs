using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
	public class Cart // sepetteki tüm ürünler
	{
		public List<CartLine> Lines { get; set; } 
		public Cart() 
		{
			Lines = new List<CartLine>();
		}
		public virtual void AddItem(Product product, int quantity)
		{
			CartLine? line = Lines.Where(p=> p.Product.ProductId.Equals(product.ProductId)).FirstOrDefault();
			//CartLine türünde yeni bir line oluştur ve Lines' lar arasında ProductId' leri birbirine eşit
			//olanları bul. Eğer line boşsa ürün ekle, line boş değilse miktarını bir artır.
			if(line is null)
			{
				Lines.Add(new CartLine()
				{
					Product = product,
					Quantity = quantity
				});
			}
			else
			{
				line.Quantity += quantity;
			}
		}
		public virtual void RemoveLine(Product product)
		{
			Lines.RemoveAll(p => p.Product.ProductId.Equals(product.ProductId));
		}
		public decimal ComputeTotalValue() => Lines.Sum(p => p.Product.Price * p.Quantity);
		//decimal, total value' yi hesaplayıp dönüş yapcaz
		public virtual void Clear() => Lines.Clear();
	}
}
