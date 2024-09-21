using CCG.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CCG.WebApi.Infrastructure.Services
{
	public class ApplicationEnvironment(IWebHostEnvironment env) : IApplicationEnvironment
	{
		public string GetContentRootPath() => env.ContentRootPath;

		public string GetWebRootPath() => env.WebRootPath;

		public string GetEnvironmentName() => env.EnvironmentName;

		public bool IsDevelopment()
		{
			return env.IsDevelopment();
		}
    
		public bool IsProduction()
		{
			return env.IsProduction();
		}

		public bool IsLocalhost()
		{
			return env.EnvironmentName.Contains("Localhost");
		}

		public bool IsEnvironment(string envName)
		{
			return env.IsEnvironment(envName);
		}
    
		public bool IsStage()
		{
			return env.IsEnvironment("Staging");
		}
    
		public bool IsPts()
		{
			return env.IsEnvironment("Pts");
		}
    
		public bool IsRc()
		{
			return env.IsEnvironment("Rc");
		}

		public string CurrentDomainServer { get; set; }
	}
}