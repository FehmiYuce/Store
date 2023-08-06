using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Components
{
	public class UserSummaryViewComponent : ViewComponent
	{
		private readonly IServiceManager _serviceManager;

		public UserSummaryViewComponent(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}
		public string Invoke()
		{
			return _serviceManager.AuthService.GetAllUsers().Count().ToString();
		}
	}
}
