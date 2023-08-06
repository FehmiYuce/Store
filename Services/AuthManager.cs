using AutoMapper;
using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class AuthManager : IAuthService
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IMapper _mapper;

		public AuthManager(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IMapper mapper)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_mapper = mapper;
		}

		public IEnumerable<IdentityRole> Roles => _roleManager.Roles;

		public async Task<IdentityResult> CreateUser(UserDtoForCreation userDto)
		{
			var user = _mapper.Map<IdentityUser>(userDto);
			var result = await _userManager.CreateAsync(user, userDto.Password); //kullanıcı oluşturduk

			if (!result.Succeeded)
				throw new Exception("User could not created");
			if (userDto.Roles.Count > 0) //eğer rolsüz kullanıcı varsa
			{
				var roleResult = await _userManager.AddToRolesAsync(user, userDto.Roles); //rol ekledik
				if (!roleResult.Succeeded)
					throw new Exception("System have problems with roles.");
			}
			return result;
		}

		public async Task<IdentityResult> DeleteOneUser(string userName)
		{
			var user = await GetOneUser(userName);
			return await _userManager.DeleteAsync(user);

		}

		public IEnumerable<IdentityUser> GetAllUsers() => _userManager.Users.ToList();

		public async Task<IdentityUser> GetOneUser(string userName)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user is not null)
			{
				return user;
			}
			throw new Exception("User could not be found");
		}

		public async Task<UserDtoForUpdate> GetOneUserForUpdate(string userName)
		{
			var user = await GetOneUser(userName);

			var userDto = _mapper.Map<UserDtoForUpdate>(user);
			userDto.Roles = new HashSet<string>(Roles.Select(x => x.Name).ToList());//tüm rolleri aldık
			userDto.UserRoles = new HashSet<string>(await _userManager.GetRolesAsync(user));//kullanıcıya bir rol atadık
																							//UserRoles UserDtoForUpdate' den geliyor.
			return userDto;


		}

		public async Task<IdentityResult> ResetPassword(ResetPasswordDto passwordDto)
		{
			var user = await GetOneUser(passwordDto.UserName);

			await _userManager.RemovePasswordAsync(user);
			var result = await _userManager.AddPasswordAsync(user, passwordDto.Password);
			return result;

		}

		public async Task UpdateUser(UserDtoForUpdate userDto)
		{
			var user = await GetOneUser(userDto.UserName);
			user.PhoneNumber = userDto.PhoneNumber;
			user.Email = userDto.Email;


			var result = await _userManager.UpdateAsync(user);
			if (userDto.Roles.Count > 0) //count ifadesi seçtiğimiz checkboxlara göre dolacak
			{
				var userRoles = await _userManager.GetRolesAsync(user); //userın rollerini aldık
				var r1 = await _userManager.RemoveFromRolesAsync(user, userRoles);//userın rollerini kaldırdık
				var r2 = await _userManager.AddToRolesAsync(user, userDto.Roles); //usera yeni rollerini ekledik.
			}
			return;


		}
	}
}
