using Client.Account.Menus;
using Client.StartMenu.Menus;
using Client.Statistics.Menus;

namespace Client.StartMenu.Extensions;

public static class ServiceCollectionExtensions
{
    public static IStartMenu CreateStartMenu(this IStatisticsMenu statisticsMenu, IAccountMenu accountMenu)
    {
        ArgumentNullException.ThrowIfNull(accountMenu);
        ArgumentNullException.ThrowIfNull(statisticsMenu);
        
        return new Menus.StartMenu(accountMenu, statisticsMenu);
    }
}