using CCG.Application.Contracts.Identity;
using CCG.Application.Contracts.Sessions;
using CCG.Application.Modules.AutoMapProfiles;
using CCG.Application.Modules.Sessions;
using CCG.Application.Modules.SharedLogger;
using CCG.Application.Services.Identity;
using CCG.Application.Utilities;
using CCG.Shared.Abstractions.Common.Logger;
using CCG.Shared.Abstractions.Game.Context;
using CCG.Shared.Game.Context;
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
            
            InstallShared(services);
        }

        private static void InstallShared(IServiceCollection services)
        {
            services.AddNonLazySingleton<ISharedLogger, AppSharedLogger>();
            services.AddSingleton<ISharedTime, SharedTime>();
            services.AddSingleton<ISharedConfig, SharedConfig>();
            services.AddSingleton<IDatabase, Database>();
        }
    }
}