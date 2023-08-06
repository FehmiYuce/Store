using Entities.Models;

namespace StoreApp.Models
{
	public class ProductListViewModel
	{
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();// başladığı yerde boş bir listeyi referans alsın
        public Pagination Pagination { get; set; } = new();
        public int TotalCount => Products.Count();
    }
}
