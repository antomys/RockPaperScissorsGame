﻿using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Data.Context;

namespace Server.Data.Extensions;

public static class DatabaseExtension
{
    private const string DatabaseConnection = "DatabaseConnection";

    public static IServiceCollection AddDatabase(this IServiceCollection service, IConfiguration configuration)
    {
        return service.AddDbContext<ServerContext>(
            builder => builder.UseSqlite
            (configuration.GetConnectionString(DatabaseConnection),
                optionsBuilder => optionsBuilder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)), 
            ServiceLifetime.Transient);
    }
}