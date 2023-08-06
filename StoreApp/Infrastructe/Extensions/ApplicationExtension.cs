using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace StoreApp.Infrastructe.Extensions
{
	public static class ApplicationExtension
	{
		//this genişletmek istediğimiz metot
		public static void ConfigureAndCheckMigration(this IApplicationBuilder app)
		{
			RepositoryContext context = app
				.ApplicationServices
				.CreateScope()
				.ServiceProvider
				.GetRequiredService<RepositoryContext>();
			if (context.Database.GetPendingMigrations().Any())
			{
				context.Database.Migrate();
			}
			//GetPendingMigrations() eğer databasede olmayan migration varsa alır.
			//artık update-database dememize gerek yok, program çalıştığında veritabanında bir değişiklik varsa kendisi migration alacak
		}
		public static void ConfigureLocalization(this WebApplication app)
		{
			app.UseRequestLocalization(options =>
			{
				options.AddSupportedCultures("tr-TR").AddSupportedUICultures("tr-TR").SetDefaultCulture("tr-TR");
				//yerel bir program yaptığımız için türkçe 
			});
		}
		public static async void ConfigureDefaultAdminUser(this IApplicationBuilder app)
		{
			const string adminUser = "Admin";
			const string adminPassword = "Admin+123456";

			// UserManager
			UserManager<IdentityUser> userManager = app
				.ApplicationServices
				.CreateScope()
				.ServiceProvider
				.GetRequiredService<UserManager<IdentityUser>>();

			// RoleManager
			RoleManager<IdentityRole> roleManager = app
				.ApplicationServices
				.CreateAsyncScope()
				.ServiceProvider
				.GetRequiredService<RoleManager<IdentityRole>>();

			IdentityUser user = await userManager.FindByNameAsync(adminUser);
			if (user is null)
			{
				user = new IdentityUser()
				{
					Email = "xxx@gmail.com",
					PhoneNumber = "123456789",
					UserName = adminUser,
				};

				var result = await userManager.CreateAsync(user, adminPassword);

				if (!result.Succeeded)
					throw new Exception("Admin user could not been created.");

				var roleResult = await userManager.AddToRolesAsync(user,roleManager.Roles.Select(r => r.Name).ToList());

				if (!roleResult.Succeeded)
					throw new Exception("System have problems with role defination for admin.");
			}
		}
	}
}
