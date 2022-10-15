using System;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
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
            // options.IncludeXmlComments($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "RPC Host",
                Version = "v1"
            });

        var jwtSecurityScheme = new OpenApiSecurityScheme 
        { 
            BearerFormat = "JWT",
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
}