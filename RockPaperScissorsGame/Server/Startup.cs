using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Server.Models;
using Server.Models.Interfaces;
using Server.Services;
using Services;

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
            
            // to dynamically change everytime. TODO
            
            services.AddTransient<IAccount, Account>();
            services.AddTransient<IStatistics, Statistics>();
            //services.AddSingleton<IStorage<IAccount>,AccountManager<IAccount>>();
            services.AddSingleton(typeof(IDeserializedObject<>), typeof(DeserializedObject<>)); 
            services.AddTransient(typeof(IStorage<>), typeof(Storage<>));
           
            services.AddSingleton<IAccountManager, AccountManager>();

            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Server", Version = "v1"}); });
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
                endpoints.MapControllers();

                endpoints.Map("/", async context =>
                {
                    var service = context.RequestServices.GetRequiredService<IDeserializedObject<Account>>();  //todo: remove

                    var dictionary = service.ConcurrentDictionary;
                    foreach (var value in dictionary.Values)
                    {
                        await context.Response.WriteAsync($"{value.Login};{value.Password}\n");
                    }
                });
                endpoints.Map("/name", async context =>
                {
                    var service = context.RequestServices.GetRequiredService<IDeserializedObject<Account>>();  //todo: remove
                    
                    var listOfStats =
                        context.RequestServices.GetRequiredService<IDeserializedObject<Statistics>>(); //todo: remove
                    
                    var accStorage = context.RequestServices.GetRequiredService <IStorage<Account>>();
                    var statStorage = context.RequestServices.GetRequiredService<IStorage<Statistics>>();
                   
                    var login = context.Request.Query["from"].FirstOrDefault();
                    var pass = context.Request.Query["to"].FirstOrDefault();

                    var user = new Account()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Login = login,
                        Password = pass
                    };

                    var stat = new Statistics
                    {
                        Id = user.Id,
                        Wins = 43,
                        Loss = 4354,
                        WinLossRatio = 21.4,
                        TimeSpent = default,
                        UsedRock = 0,
                        UsedPaper = 0,
                        UsedScissors = 0,
                        Points = 0,
                        Login = user.Login
                    };

                    accStorage.Add(user);
                    statStorage.Add(stat);
                    
                    var dictionary = service.ConcurrentDictionary;
                    foreach (var value in dictionary.Values)
                    {
                        await context.Response.WriteAsync($"LOGIN: {value.Login};{value.Password}\n");
                    }
                    var dictionary2 = listOfStats.ConcurrentDictionary;
                    foreach (var value in dictionary2.Values)
                    {
                        await context.Response.WriteAsync($"STATS: {value.Id};{value.Id}\n");
                    }

                    await service.UpdateData();
                    await listOfStats.UpdateData();
                });
            });
        }
    }
}