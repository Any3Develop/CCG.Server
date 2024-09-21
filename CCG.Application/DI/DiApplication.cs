using CCG.Application.Modules.AutoMapProfiles;
using CCG.Application.Modules.SharedLogger;
using CCG.Application.Utilities;
using CCG.Shared.Abstractions.Common.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace CCG.Application.DI
{
    public static class DiApplication
    {
        public static void InstallApplication(this IServiceCollection services)
        {
            services.AddAutoMapper((serviceProvider, builder) => builder.AddProfile(new AutoMapProfile(serviceProvider)), Array.Empty<Type>());
            services.AddNonLazySingleton<ISharedLogger, AppSharedLogger>();
        }
    }
}