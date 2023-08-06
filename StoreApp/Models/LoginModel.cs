using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models
{
	public class LoginModel
	{
		private string? _returnurl; //eğer kullanıcı giriş yapmamış ve ürün almaya çalıştıysa login sayfasına yönlendirmek için
		[Required(ErrorMessage ="Name is required.")]
		public string? Name { get; set; }
		[Required(ErrorMessage = "Password is required.")]
		public string? Password { get; set; }
		public string ReturnUrl
		{
			get
			{
				if (_returnurl == null)
					return "/"; //anasayfaya gönder
				else
					return _returnurl; //geldiği yere gönder
			}
			set 
			{ 
				_returnurl = value; 
			}
		}
	}
}
