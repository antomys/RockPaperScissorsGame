using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Authentication.Extensions;
using Server.Dal.Context;
using Server.Dal.Extensions;
using Server.Host.Extensions;

namespace Server.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services   //.AddServices()
                .AddDatabase(Configuration)
                .AddSwagger()
                .AddAuthentications();
            
            // Adding services
            services.AddServices();
            
            services.AddControllers();
           
            services.AddCors();
        }
        
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ServerContext serverContext)
        {
            serverContext?.Database.Migrate();
            serverContext?.EnsureBotCreated();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server.Host v1"));
            }
            
            app.UseCors(builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Authorization", "Accept", "Content-Type", "Origin"));

            app.UseMiddleware<LoggingMiddleware>();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}