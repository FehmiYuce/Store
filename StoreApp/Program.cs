using StoreApp.Infrastructe.Extensions;

namespace StoreApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly); //api service. Controllerlarý kullanýrken artýk ilgili referansa da bakmýþ olacak. 
			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages();

			//ServiceExtension' dan geliyorlar
			builder.Services.ConfigureDbContext(builder.Configuration);//yazdýðýmýz extension metodlarýmýz
			builder.Services.ConfigureIdentity();
			builder.Services.ConfigureSession();// parametre vermememizin sebebi this ile ifade edilen yapý geniþlettiðimiz metod
			// eðer 2. bir parametre varsa yukarýdaki gibi onu vermemiz gerekir.
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

            app.UseAuthentication(); //oturum açma 
            app.UseAuthorization(); //yetkilendirme
			//routing ile endpoint arasýna koyma sebebimiz logout' un çalýþmamasý. middleware yapýlarýný çaðýrýrken sýralama önemli olabilir.



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
				endpoints.MapControllers(); //api' yi routere tanýmladýk

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