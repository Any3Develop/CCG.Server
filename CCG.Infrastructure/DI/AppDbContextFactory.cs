using CCG.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CCG.Infrastructure.DI
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite("Data Source=ccg.demo.db", o =>
            {
                o.CommandTimeout(360);
                o.MigrationsHistoryTable("__EFMigrationsHistory", "public");
            });

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}