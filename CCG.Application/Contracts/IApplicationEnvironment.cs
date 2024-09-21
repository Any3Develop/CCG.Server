namespace CCG.Application.Contracts
{
	public interface IApplicationEnvironment
	{
		public string GetContentRootPath();
		public string GetWebRootPath();
		public string GetEnvironmentName();
		public bool IsDevelopment();
		public bool IsProduction();
		public bool IsLocalhost();
		public bool IsEnvironment(string envName);
		public bool IsStage();
		public bool IsPts();
		public bool IsRc();
		public string CurrentDomainServer { get; set; }
	}
}