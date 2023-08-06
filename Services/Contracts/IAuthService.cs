using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IAuthService
    {
        IEnumerable<IdentityRole> Roles { get; }
        IEnumerable<IdentityUser> GetAllUsers();
        Task<IdentityResult> CreateUser(UserDtoForCreation userDto);
        Task<UserDtoForUpdate> GetOneUserForUpdate(string userName); //userı güncellerken rolleri de güncellememiz gerektiği için bu metodu tanımladık.
        Task<IdentityUser> GetOneUser(string userName);
        Task UpdateUser(UserDtoForUpdate userDto);
		Task<IdentityResult> ResetPassword(ResetPasswordDto passwordDto);
		//string de alabilirdik ama IdentityResult validation vs vs yönetim bakımından daha kullanışlı
        Task<IdentityResult> DeleteOneUser(string userName);
	}
}
