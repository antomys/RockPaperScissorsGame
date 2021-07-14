using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Dal.Context;

namespace Server.Extensions
{
    public static class DatabaseExtension
    {
        private const string DatabaseConnection = "DatabaseConnection";
        private const string MigrationAssemblyName = "Server.Dal";
        public static IServiceCollection AddDatabase(this IServiceCollection service, IConfiguration configuration)
        {
            return service.AddDbContext<ServerContext>(
                builder => builder.UseSqlite
                (configuration.GetConnectionString(DatabaseConnection),
                    x => x.MigrationsAssembly(MigrationAssemblyName)), 
                ServiceLifetime.Transient);
        }
    }
}