using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Server.Extensions
{
    public static class SwaggerExtension
    {
        //todo: add here documentation
        public static IServiceCollection AddSwagger(this IServiceCollection service)
        {
            return service.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Server", Version = "v1"});
            });
        }
    }
}