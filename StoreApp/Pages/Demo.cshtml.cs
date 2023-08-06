using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StoreApp.Pages
{
   
    public class DemoModel : PageModel
    {
        public String? FullName => HttpContext?.Session?.GetString("name") ?? "";
        //readonly bir FullName. Her seferinde kullan�c�dan oturum a�mas� i�in bilgi istemeyelim diye b�yle bir yap� kullan�ld�
		public void OnGet()
        {
        }
        public void OnPost([FromForm]string name) //bu string name formdan geliyor, name tag'inin i�erisindeki name
		{
            //FullName = name;
            //E�er bunu class olarak tutarsak kullan�c� bir ba�ka sayfaya ge�ip geri geldi�inde ad�n� g�remez, oturumunun kapanmas�n� istemiyoruz
            HttpContext.Session.SetString("name", name);
        }
		
	}
}
