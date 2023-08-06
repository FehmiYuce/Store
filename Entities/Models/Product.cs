using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; } = String.Empty;
        public decimal Price { get; set; }
        public String? Summary { get; set; } = String.Empty;
        public String? ImageUrl { get; set; }
        public int? CategoryId { get; set; } // Foreign key
        // kategorideki 1 Id yi artık productta istediğimiz kadar kullanabiliriz.
        public Category? Category { get; set; }  // navigation property. fiziksel bir kayıt değil. ilişki için kullanacağız
        public bool ShowCase { get; set; } //vitrin

    }
}
