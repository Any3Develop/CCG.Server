namespace CCG.Infrastructure.Persistence.DbSeed
{
    public interface IDbSeedService
    {
        Task Seed();

        Task Migrate();

        Task CleanUp();
    }
}
