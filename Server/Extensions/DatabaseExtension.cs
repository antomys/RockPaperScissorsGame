using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Dal.Context;

namespace Server.Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection service, IConfiguration configuration)
        {
            return service.AddDbContext<ServerContext>(
            builder => builder.UseSqlite
            (configuration.GetConnectionString("connectionString"),
                x=> x.MigrationsAssembly("Server.Dal")));
        }
    }
}