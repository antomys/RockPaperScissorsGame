using Microsoft.EntityFrameworkCore;
using Server.Authentication.Extensions;
using Server.Bll.Extensions;
using Server.Bll.Options;
using Server.Data.Context;
using Server.Data.Extensions;
using Server.Host.Extensions;

namespace Server.Host;

public sealed class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHealthChecks();

        services
            .Configure<CleanerOptions>(Configuration.GetRequiredSection(CleanerOptions.Section));

        services
            .AddDatabase(Configuration)
            .AddSwagger()
            .AddAuthentications();

        services.AddBusinessLogic();

        services.AddControllers();

        services.AddCors();
    }

    public static void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        ServerContext serverContext)
    {
        serverContext.Database.Migrate();
        serverContext?.EnsureBotCreated();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
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
            endpoints.MapHealthChecks("/health");
            endpoints.MapControllers();
        });
    }
}