using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Authentication.Services;

namespace Server.Authentication.Extensions;

/// <summary>
///     Authentication services extension.
/// </summary>
public static class AuthenticationExtension
{
    /// <summary>
    ///     Registers authentication.
    /// </summary>
    /// <param name="services">Service collection.</param>
    public static IServiceCollection AddAuthentications(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions<AuthOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = services
                    .BuildServiceProvider()
                    .GetRequiredService<IOptions<AuthOptions>>()
                    .Value;
                            
                options.RequireHttpsMetadata = jwtOptions.RequireHttps;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true
                };
            });

        services.AddTransient<IAuthService, AuthService>();

        return services;
    }
}