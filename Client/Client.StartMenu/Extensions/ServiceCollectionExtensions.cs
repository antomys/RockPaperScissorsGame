using Client.Account.Menus;
using Client.StartMenu.Menus;
using Client.StartMenu.Services;
using Client.Statistics.Menus;
using RockPaperScissors.Common.Client;

namespace Client.StartMenu.Extensions;

public static class ServiceCollectionExtensions
{
    public static IStartMenu CreateStartMenu(
        this IStatisticsMenu statisticsMenu,
        IAccountMenu accountMenu,
        IHealthCheckService healthCheckService)
    {
        ArgumentNullException.ThrowIfNull(accountMenu);
        ArgumentNullException.ThrowIfNull(statisticsMenu);

        return new Menus.StartMenu(accountMenu, statisticsMenu, healthCheckService);
    }
    
    public static IHealthCheckService CreateHealthCheckService(
        this IClient client,
        CancellationTokenSource cancellationTokenSource)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(cancellationTokenSource);

        return new HealthCheckService(client, cancellationTokenSource);
    }
}