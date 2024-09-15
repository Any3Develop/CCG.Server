using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CCG.WebApi.Infrastructure.Middleware
{
    public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var internalResult = JsonConvert.SerializeObject(new
                {
                    error.Message, error.Source,
                    Stack = error.StackTrace?.Split("\n"),
                    Inner = error.InnerException?.Message,
                    InnerSource = error.InnerException?.Source
                });

                response.StatusCode = 500;

                var prop = error.GetType().GetProperty("StatusCode")!;
                if (prop != null)
                {
                    var status = prop.GetValue(error);
                    if (status is int code)
                    {
                        response.StatusCode = code;
                    }
                }

                if (response.StatusCode >= 500) 
                    logger.LogCritical(internalResult);


                await response.WriteAsync(internalResult);
            }
        }
    }
}