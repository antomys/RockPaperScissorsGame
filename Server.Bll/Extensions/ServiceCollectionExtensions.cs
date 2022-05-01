using Microsoft.Extensions.DependencyInjection;
using Server.Bll.HostedServices;
using Server.Bll.Services;
using Server.Bll.Services.Interfaces;

namespace Server.Bll.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection service)
    {
        service
            .AddTransient<IStatisticsService,StatisticsService>()
            .AddTransient<ILongPollingService,LongPollingService>()
            .AddHostedService<CleanerHostedService>();
        service.AddHttpContextAccessor();
        
        service
            .AddTransient<IRoomService, RoomService>()
            .AddTransient<IRoundService, RoundService>();

        return service;
    }
}