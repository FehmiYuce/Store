using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
	public record ResetPasswordDto : UserDto
	{
        public String? UserName { get; set; }

        [DataType(DataType.Password)] //indexte input type'ı manuel olarak belirlememize gerek yok artık. doğrudan bununla ilişkili inputun şifre olmasını sağlayacak
        [Required(ErrorMessage ="Password is required.")]
        public String? Password { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage ="ConfirmPassword is required.")]
        [Compare("Password",ErrorMessage ="Password and ConfirmPassword must be match.")]
        public String? ConfirmPassword { get; set; }
    }
}
