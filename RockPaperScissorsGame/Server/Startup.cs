using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Server.GameLogic.LogicServices;
using Server.GameLogic.LogicServices.Impl;
using Server.Services;
using Server.Services.Interfaces;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddSingleton(typeof(IDeserializedObject<>), typeof(DeserializedObject<>)); 
            services.AddTransient(typeof(IStorage<>), typeof(Storage<>));

            services.AddSingleton<IAccountManager, AccountManager>();
            services.AddSingleton<IRoomCoordinator, RoomCoordinator>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Server", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/status/{sessionId}", async context =>
                {
                    var service = context.RequestServices.GetRequiredService<IAccountManager>();  //todo: remove
                    
                    var sessionId = (string) context.Request.RouteValues["sessionId"];

                    if (sessionId == null)
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    }
                    else  if (await service.IsActive(sessionId))
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.OK;
                    }
                    else
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                    }
                });
                
                endpoints.MapControllers();
                
            });
        }
    }
}