namespace StoreApp.Models
{
	public class Pagination
	{
        public int TotalItems { get; set; } //sergilenen tüm ürünler
        public int CurrentPage { get; set; } 
        public int ItemsPerPage { get; set; } 
        public int TotalPages  => (int)Math.Ceiling((decimal)TotalItems/ItemsPerPage); 
    }
}
