using Microsoft.Extensions.DependencyInjection;

namespace CCG.Application.Utilities
{
    public static class DiNonLazySingletonExtension
    {
        private static List<Type> nonLazySingletons = new();

        public static void AddNonLazySingleton<TService, TImplementation>(this IServiceCollection service)
            where TService : class
            where TImplementation : class, TService
        {
            if (nonLazySingletons == null)
                throw new InvalidOperationException($"Can't add singleton after app was build.");
            
            nonLazySingletons.Add(typeof(TService));
            service.AddSingleton<TService, TImplementation>();
        }

        public static void EnsureNonLazySingletones(this IServiceProvider service)
        {
            if (nonLazySingletons == null)
                throw new InvalidOperationException($"Can't build singletons twice.");
            
            foreach (var serviceType in nonLazySingletons) 
                service.GetRequiredService(serviceType);
            
            nonLazySingletons.Clear();
            nonLazySingletons = null;
        }
    }
}