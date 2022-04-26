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

        // In this way I am registering multiple interfaces to one Transient instance of RoomService;
        service
            .AddTransient<RoomService>()
            .AddTransient<IRoomService>(provider => provider.GetRequiredService<RoomService>())
            .AddTransient<IHostedRoomService>(provider => provider.GetRequiredService<RoomService>());

        service
            .AddTransient<IRoundService, RoundService>();

        return service;
    }
}