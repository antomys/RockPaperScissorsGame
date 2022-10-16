using Client.Account.Services;
using Client.Account.Services.Interfaces;
using Client.Statistics.Menus;
using Client.Statistics.Services;
using RockPaperScissors.Common.Client;

namespace Client.Statistics.Extensions;

public static class ServiceCollectionExtensions
{
    public static IStatisticsService CreateStatisticsService(this IClient client)
    {
        return new StatisticsService(client);
    }
    
    public static IStatisticsMenu CreateStatisticsMenu(this IStatisticsService statisticsService, IAccountService accountService)
    {
        return new StatisticsMenu(statisticsService, accountService);
    }
}