using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Server.Host.Extensions;

/// <summary>
/// Swagger extension
/// </summary>
public static class SwaggerExtension
{
    /// <summary>
    /// Registers swagger.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>Service collection.</returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSwaggerGen(options =>
        {
            var title = AppDomain.CurrentDomain.FriendlyName;
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            var version = assemblyName.Version?.ToString() ?? string.Empty;
            var documentationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyName.Name}.xml");

            options.IncludeXmlComments(documentationPath);
            options.SwaggerDoc(version, new OpenApiInfo
            {
                Title = title,
                Version = version
            });

        var jwtSecurityScheme = new OpenApiSecurityScheme
        { 
            BearerFormat = JwtConstants.TokenType,
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Put **_ONLY_** your JWT Bearer token on text box below!",

            Reference = new OpenApiReference
            {
                 Id = JwtBearerDefaults.AuthenticationScheme,
                 Type = ReferenceType.SecurityScheme
            }
        };

        options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                jwtSecurityScheme,
                Array.Empty<string>()
            }
        });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerUI(
        this IApplicationBuilder applicationBuilder)
    {
        ArgumentNullException.ThrowIfNull(applicationBuilder);

        applicationBuilder.UseSwaggerUI(swaggerUiOptions =>
        {
            var title = AppDomain.CurrentDomain.FriendlyName;
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
            swaggerUiOptions.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{title} v{version}");
        });

        return applicationBuilder;
    }
}