using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;
using Services;
using Entities.Models;
using StoreApp.Models;
using Microsoft.AspNetCore.Identity;
using StoreApp.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace StoreApp.Infrastructe.Extensions
{
	//Serviceleri özelleştirmek için kullanılır.
	public static class ServiceExtension
	{
		public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<RepositoryContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("mssqlconnection"), b => b.MigrationsAssembly("StoreApp"));
				options.EnableSensitiveDataLogging(true);//oturum bilgileri gibi hasssas bilgileri görmemizi sağlar. Geliştirme sürecinde uygulamayı kontrol etmek, gelen  giden logları görmek içindir. Daha sonra değiştirebiliriz.
			});
		}
		public static void ConfigureIdentity(this IServiceCollection services)
		{
			//IdentityUser en temel sınıflardan
			services.AddIdentity<IdentityUser, IdentityRole>(options =>
			{
				options.SignIn.RequireConfirmedAccount = false; //email doğrulanmasına gerek yok.
				options.User.RequireUniqueEmail = true; //her kullanıcının farklı emaili olsun
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireDigit = false;
				options.Password.RequiredLength = 6;
			}).AddEntityFrameworkStores<RepositoryContext>(); //saklamak için kendi veritabanımızı kullanacağız
		}
		public static void ConfigureSession(this IServiceCollection services)
		{
			services.AddDistributedMemoryCache();
			//artık altta HttpContextAccessor sayesinde istediğimiz configurationları yapabilcez.
			services.AddSession(options =>
			{
				options.Cookie.Name = "StoreApp.Session";
				options.IdleTimeout = TimeSpan.FromMinutes(10); //kullanıcı 10 dk içinde başka requestte bulunmazsa oturumu düşürülür.
			});
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //oturum okumamıza yarayan accessor yapısını da buraya atabiliriz.
			services.AddScoped<Cart>(c => SessionCart.GetCart(c));
			//AddSingleton iken herkes aynı sepeti kullanır. Bir tane cart nesnesi newlenecek.
			// Sorun, her kullanıcının aynı sepeti olacak. Eğer scoped tanımlarsak her kullanıcının farklı sepeti olacak.
		}
		public static void ConfigureRepositoryRegistration(this IServiceCollection services)
		{
			services.AddScoped<IRepositoryManager, RepositoryManager>();
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();
			//IoC kayıtları. Inversion of control. Configuration işlemlerini bu şekilde yönetiriz. Kayıt işlemi
		}
		public static void ConfigureServiceRegistration(this IServiceCollection services)
		{
			services.AddScoped<IServiceManager, ServiceManager>();
			services.AddScoped<IProductService, ProductManager>();
			services.AddScoped<ICategoryService, CategoryManager>();
			services.AddScoped<IOrderService, OrderManager>();
			services.AddScoped<IAuthService, AuthManager>();
		}
		public static void ConfigureApplicationCookie(this IServiceCollection services)
		{
			//sitemizi yönetmek için
			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = new PathString("/Account/Login");
				options.AccessDeniedPath = new PathString("/Account/AccessDenied");
				options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
				options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
			});
		}
		public static void ConfigureRouting(this IServiceCollection services)
		{
			services.AddRouting(options =>
			{
				options.LowercaseUrls = true; 
				options.AppendTrailingSlash = false; //url' de her sayfanın sonuna / ekler. False iken eklemez.
			});
		}
	}
}
