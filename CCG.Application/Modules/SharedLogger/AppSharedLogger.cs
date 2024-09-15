using CCG.Shared.Abstractions.Common.Logger;
using Microsoft.Extensions.Logging;

namespace CCG.Application.Modules.SharedLogger
{
	public class AppSharedLogger : ISharedLogger
	{
		private readonly ILogger<AppSharedLogger> logger;

		public AppSharedLogger(ILogger<AppSharedLogger> logger)
		{
			this.logger = logger;
			Shared.Common.Logger.SharedLogger.Initialize(this);
		}
		public void Log(object message)
		{
			logger.LogDebug("{message}", message);
		}

		public void Warning(object message)
		{
			logger.LogWarning("{message}", message);
		}

		public void Error(object message)
		{
			logger.LogCritical("{message}", message);
		}
	}
}