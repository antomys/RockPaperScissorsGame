﻿using System;
using System.Reflection;
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
        if (services is null) throw new ArgumentNullException(nameof(services));

        services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "RPC Host", Version = "v1" });

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            },
                        },
                        Array.Empty<string>()
                    }
                });

            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    Name = "Authorization",
                    Description = "JWT token",
                    BearerFormat = "JWT"
                });
        });

        return services;
    }
}