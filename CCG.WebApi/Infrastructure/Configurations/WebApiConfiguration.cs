﻿using CCG.Application.DI;
using CCG.Application.Services;
using CCG.Infrastructure.Persistence.DbSeed;
using CCG.WebApi.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CCG.WebApi.Infrastructure.Configurations
{
    public static class WebApiConfiguration
    {
        public static async Task ConfigureWebApiAsync(this IApplicationBuilder app, IServiceProvider services)
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();

            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./v1/swagger.json", $"{VersionInfo.SolutionName} API {VersionInfo.ApiVersion}");
                c.DocumentTitle = $"{VersionInfo.SolutionName}";
                c.RoutePrefix = "swagger";
            });

            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCaching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                foreach (var route in endpoints.DataSources.SelectMany(ds => ds.Endpoints))
                {
                    Console.WriteLine($"Маршрут: {route.DisplayName}");
                }
            });
            services.EnsureNonLazySingletones(); // create all non lazy singletons.

            using var scope = services.CreateScope();
            var servicesScoped = scope.ServiceProvider;
            var env = servicesScoped.GetRequiredService<IApplicationEnvironment>();
            var dbSeederService = servicesScoped.GetRequiredService<IDbSeedService>();
            
            if (env.IsProduction())
                await dbSeederService.Migrate();
                
            await dbSeederService.Seed();
            await dbSeederService.CleanUp();
        }
    }
}