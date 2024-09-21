using Microsoft.Extensions.DependencyInjection;
using CCG.Application.Contracts.Persistence;
using CCG.Domain.Entities.Identity;
using CCG.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CCG.Infrastructure.DI
{
    public static class DiInfrastructure
    {
        public static void InstallInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("SQLiteDbConnection"), o =>
                {
                    o.CommandTimeout(360);
                    o.MigrationsHistoryTable("__EFMigrationsHistory", "public");
                });
            });
            
            service.AddDefaultIdentity<UserEntity>(o =>
            {
                o.SignIn.RequireConfirmedAccount = false;
                o.Password.RequireDigit = false;
                o.Password.RequireUppercase = false;
                o.Password.RequiredLength = 4;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.User.RequireUniqueEmail = false;
                o.User.AllowedUserNameCharacters = string.Empty;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
            service.AddScoped<IAppDbContext, AppDbContext>(provider => provider.GetService<AppDbContext>());
        }
    }
}