using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace CCG.Application.Utilities
{
    public static class DiNonLazySingletonExtension
    {
        private record NonLazyBinder(Type ServiceType, Action<object> Options);
        private static ConcurrentBag<NonLazyBinder> nonLazyBinders = new();

        public static void AddNonLazySingleton<TService, TImplementation>(this IServiceCollection service, Action<TImplementation> options = null)
            where TService : class
            where TImplementation : class, TService
        {
            if (nonLazyBinders == null)
                throw new InvalidOperationException($"Can't register singleton after app was build.");
            
            nonLazyBinders.Add(new NonLazyBinder(typeof(TService), impl => options?.Invoke((TImplementation)impl)));
            service.AddSingleton<TService, TImplementation>();
        }

        public static void EnsureNonLazySingletones(this IServiceProvider service)
        {
            while (true)
            {
                if (nonLazyBinders == null) 
                    throw new InvalidOperationException($"Can't build singletons twice.");

                var binds = nonLazyBinders.ToArray();
                nonLazyBinders.Clear();

                foreach (var bind in binds) 
                    bind.Options(service.GetRequiredService(bind.ServiceType));

                if (!nonLazyBinders.IsEmpty) 
                    continue;
                
                nonLazyBinders = null;
                return;
            }
        }
    }
}