using CCG.Application.DI;
using CCG.WebApi.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;

namespace CCG.WebApi.Infrastructure.Configurations
{
    public static class WebApiConfiguration
    {
        public static void ConfigureWebApi(this IApplicationBuilder app, IServiceProvider serviceProvider)
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
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            serviceProvider.EnsureNonLazySingletones(); // create all non lazy singletons.
        }
    }
}