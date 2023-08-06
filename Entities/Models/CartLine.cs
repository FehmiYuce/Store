using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
	public class CartLine // sepetteki bir ürün
	{
		public int CartLineId { get; set; }
		public Product Product { get; set; } = new(); //tanımladığımız yerde referans aldık
		public int Quantity { get; set; }
	}
}
