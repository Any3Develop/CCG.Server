using CCG.Application.Contracts.Services.Identity;
using CCG.Application.Contracts.Services.Sessions;
using CCG.Application.Modules.AutoMapProfiles;
using CCG.Application.Modules.SharedLogger;
using CCG.Application.Services.Identity;
using CCG.Application.Services.Sessions;
using CCG.Application.Utilities;
using CCG.Shared.Abstractions.Common.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace CCG.Application.DI
{
    public static class DiApplication
    {
        public static void InstallApplication(this IServiceCollection services)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ISessionFactory, SessionFactory>();
            services.AddSingleton<IRuntimeSessionRepository, RuntimeSessionRepository>();
            
            services.AddAutoMapper((serviceProvider, builder) => builder.AddProfile(new AutoMapProfile(serviceProvider)), Array.Empty<Type>());
            services.AddNonLazySingleton<ISharedLogger, AppSharedLogger>();
        }
    }
}