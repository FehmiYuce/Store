using StoreApp.Infrastructe.Extensions;

namespace StoreApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly); //api service. Controllerlar� kullan�rken art�k ilgili referansa da bakm�� olacak. 
			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages();

			//ServiceExtension' dan geliyorlar
			builder.Services.ConfigureDbContext(builder.Configuration);//yazd���m�z extension metodlar�m�z
			builder.Services.ConfigureIdentity();
			builder.Services.ConfigureSession();// parametre vermememizin sebebi this ile ifade edilen yap� geni�letti�imiz metod
			// e�er 2. bir parametre varsa yukar�daki gibi onu vermemiz gerekir.
			builder.Services.ConfigureRepositoryRegistration();
			builder.Services.ConfigureServiceRegistration();
			builder.Services.ConfigureRouting();
			builder.Services.ConfigureApplicationCookie();
			
			builder.Services.AddAutoMapper(typeof(Program));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseSession();
			app.UseRouting();

            app.UseAuthentication(); //oturum a�ma 
            app.UseAuthorization(); //yetkilendirme
			//routing ile endpoint aras�na koyma sebebimiz logout' un �al��mamas�. middleware yap�lar�n� �a��r�rken s�ralama �nemli olabilir.



#pragma warning disable ASP0014 // Suggest using top level route registrations
            app.UseEndpoints(endpoints =>
			{
				endpoints.MapAreaControllerRoute(
					name: "Admin",
					areaName: "Admin",
					pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}"
				);

				endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
				endpoints.MapControllers(); //api' yi routere tan�mlad�k

			});
#pragma warning restore ASP0014 // Suggest using top level route registrations
		
			//ApplicationExtensiondan geliyor
			app.ConfigureAndCheckMigration();
			app.ConfigureLocalization();
			app.ConfigureDefaultAdminUser();

			app.Run();
		}
	}
}