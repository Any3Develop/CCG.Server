using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CCG.WebApi.Infrastructure.ActionFilter
{
    public class ReqTokenOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "req_token",
                In = ParameterLocation.Header,
                Required = false,
            });
        }
    }
}