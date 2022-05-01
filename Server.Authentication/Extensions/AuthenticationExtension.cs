using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Authentication.Models;
using Server.Authentication.Models.Interfaces;
using Server.Authentication.Services;

namespace Server.Authentication.Extensions;

/// <summary>
/// Authentication services extension
/// </summary>
public static class AuthenticationExtension
{
    /// <summary>
    /// Register authentication
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns></returns>
    public static IServiceCollection AddAuthentications(this IServiceCollection services)
    {
        _ = services ?? throw new ArgumentNullException(nameof(services));

        services.AddOptions<AuthOptions>();
        
        var jwtOptions = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<AuthOptions>>()
            .Value;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = jwtOptions.RequireHttps;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    IssuerSigningKey = jwtOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true
                };
            });

        services
            .AddTransient<IAuthService, AuthService>()
            .AddTransient<IApplicationUser, ApplicationUser>()
            .AddSingleton(typeof(AttemptValidationService));
        
        return services;
    }
}