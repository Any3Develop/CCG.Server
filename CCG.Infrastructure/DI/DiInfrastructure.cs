using Microsoft.Extensions.DependencyInjection;
using CCG.Application.Contracts.Persistence;
using CCG.Domain.Entities.Identity;
using CCG.Infrastructure.Persistence;
using CCG.Infrastructure.Persistence.DbSeed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CCG.Infrastructure.DI
{
    public static class DiInfrastructure
    {
        public static void InstallInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = $"Data Source={Path.Combine(AppContext.BaseDirectory.Replace(@"CCG.WebApi\bin\Debug\net8.0","CCG.Infrastructure"), "ccg.demo.db")}";
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(connectionString, o =>
                {
                    o.CommandTimeout(360);
                    o.MigrationsHistoryTable("__EFMigrationsHistory", "public");
                });
            });
            
            services.AddDefaultIdentity<UserEntity>(o =>
            {
                o.SignIn.RequireConfirmedAccount = false;
                
                o.Password.RequireDigit = false;
                o.Password.RequireUppercase = false;
                o.Password.RequiredLength = 4;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;
                
                o.User.RequireUniqueEmail = false;
                o.User.AllowedUserNameCharacters = string.Empty;
                
                o.Lockout.MaxFailedAccessAttempts = 5;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                o.Lockout.AllowedForNewUsers = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
            
            services.AddScoped<IAppDbContext, AppDbContext>(provider => provider.GetService<AppDbContext>());
            services.AddScoped<IDbSeedService, DbSeedService>();
        }
    }
}