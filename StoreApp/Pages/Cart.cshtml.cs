using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Contracts;
using StoreApp.Infrastructe.Extensions;

namespace StoreApp.Pages
{
    public class CartModel : PageModel
    {
        // Yorum sat�r�na al�nanlar� art�k SessionCart ger�ekle�tirdi�i i�in o tan�mlamalara gerek yok.
        private readonly IServiceManager _serviceManager;
		public Cart Cart { get; set; }
		public string ReturnUrl { get; set; } = "/";

		public CartModel(IServiceManager serviceManager, Cart cartService)
		{
			_serviceManager = serviceManager;
            Cart = cartService;
		}
        //cart da alan tan�m� yapmadan DI uygulam�� olduk. Lakin cart nesnesini new' lerserk sadece 1 tane �r�n ekleyebiliriz.
		public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";  //e�er de�er bo�sa ana sayfaya d�ner
            //Cart= HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }
        public IActionResult OnPost(int productId, string returnUrl) 
        {
            Product? product = _serviceManager.ProductService.GetOneProduct(productId, false);
            //bir tane �r�n se�iyorsak cart' a product' � ekler. product nesnesini dolduruyorsak sepete(cart' a post ediyor bizi)
            if (product is not null)
            {
				//Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart(); //Cart nesnesini sessiondan al�yoruz. serialize edildi ve elimize bir class ge�ti
				Cart.AddItem(product,1); // Cart nesnesini ekliyoruz
                //HttpContext.Session.SetJson<Cart>("cart", Cart); //Cart nesnesini tekrar sessiona veriyoruz. Bu sayede birden fazla �r�n ekleyebilece�iz
            }
            return RedirectToPage(new { returnUrl = returnUrl }); //returnUrl
        }
        public IActionResult OnPostRemove(int id, string returnUrl)
        {
			//Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
			Cart.RemoveLine(Cart.Lines.First(cl=>cl.Product.ProductId.Equals(id)).Product);
			//HttpContext.Session.SetJson<Cart>("cart", Cart);
			return Page();
        }
    }
}
