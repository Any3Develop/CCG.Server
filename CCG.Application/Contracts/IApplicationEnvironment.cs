namespace CCG.Application.Contracts
{
    public interface IApplicationEnvironment
    {
        public string CurrentDomainServer { get; set; }
        public string GetContentRootPath();
        public string GetWebRootPath();
        public string GetEnvironmentName();
        public bool IsDevelopment();
        public bool IsProduction();
        public bool IsLocalhost();
        public bool IsEnvironment(string envName);
        public bool IsStage();
    }
}