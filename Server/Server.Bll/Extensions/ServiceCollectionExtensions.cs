using System;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Server.Bll.HostedServices;
using Server.Bll.Models;
using Server.Bll.Services;
using Server.Bll.Services.Interfaces;
using Server.Data.Entities;

namespace Server.Bll.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection service)
    {
        ArgumentNullException.ThrowIfNull(service);

        TypeAdapterConfig<Statistics, ShortStatisticsModel>
            .NewConfig()
            .Map(shortStatisticsModel => shortStatisticsModel.Login, statistics => statistics.Account.Login)
            .Map(shortStatisticsModel => shortStatisticsModel.Score, statistics => statistics.Score);
        
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