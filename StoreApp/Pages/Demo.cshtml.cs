using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StoreApp.Pages
{
   
    public class DemoModel : PageModel
    {
        public String? FullName => HttpContext?.Session?.GetString("name") ?? "";
        //readonly bir FullName. Her seferinde kullanýcýdan oturum açmasý için bilgi istemeyelim diye böyle bir yapý kullanýldý
		public void OnGet()
        {
        }
        public void OnPost([FromForm]string name) //bu string name formdan geliyor, name tag'inin içerisindeki name
		{
            //FullName = name;
            //Eðer bunu class olarak tutarsak kullanýcý bir baþka sayfaya geçip geri geldiðinde adýný göremez, oturumunun kapanmasýný istemiyoruz
            HttpContext.Session.SetString("name", name);
        }
		
	}
}
