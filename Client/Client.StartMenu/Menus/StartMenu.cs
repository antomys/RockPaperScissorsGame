using Client.Account.Menus;
using Client.StartMenu.Enums;
using Client.StartMenu.Extensions;
using Client.StartMenu.Services;
using Client.Statistics.Menus;
using RockPaperScissors.Common.Extensions;

namespace Client.StartMenu.Menus;

internal sealed class StartMenu: IStartMenu
{
    private readonly IAccountMenu _accountMenu;
    private readonly IStatisticsMenu _statisticsMenu;
    private readonly IHealthCheckService _healthCheckService;

    public StartMenu(
        IAccountMenu accountMenu,
        IStatisticsMenu statisticsMenu,
        IHealthCheckService healthCheckService)
    {
        _accountMenu = accountMenu ?? throw new ArgumentNullException(nameof(accountMenu));
        _statisticsMenu = statisticsMenu ?? throw new ArgumentNullException(nameof(statisticsMenu));
        _healthCheckService = healthCheckService ?? throw new ArgumentNullException(nameof(healthCheckService));
    }

    public async Task PrintAsync(CancellationToken cancellationToken)
    {
        PrintGreeting();

        await _healthCheckService.ConnectAsync();
        cancellationToken.ThrowIfCancellationRequested();
        await _healthCheckService.PingAsync();

        "\nPress any key to show start up menu list.".Print(ConsoleColor.Green);

        Console.ReadKey();
        Console.Clear();

        await ShowStartAsync(cancellationToken);
    }

    private async Task ShowStartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            "Start menu:".Print(ConsoleColor.DarkYellow);
            $"{MenuTypes.Account.GetValue()}. \t{MenuTypes.Account.GetDisplayName()}".Print(ConsoleColor.DarkYellow);
            $"{MenuTypes.Leaderboard.GetValue()}. \t{MenuTypes.Leaderboard.GetDisplayName()}".Print(ConsoleColor.DarkYellow);
            $"{MenuTypes.Exit.GetValue()}. \t{MenuTypes.Exit.GetDisplayName()}".Print(ConsoleColor.DarkYellow);

            "Please select an item from the list".Print(ConsoleColor.Green);

            Console.Write("Select -> ");

            var menuType = Console.ReadLine().TryGetMenuType();
            
            if (menuType is MenuTypes.Unknown)
            {
                "Invalid input. Try again.".Print(ConsoleColor.Red);
                
                continue;
            }
            
            switch (menuType)
            {
                case MenuTypes.Account:
                    await _accountMenu.StartAsync(cancellationToken);
                    Console.Clear();

                    break;
                
                case MenuTypes.Leaderboard:
                    await _statisticsMenu.StartAsync(cancellationToken);
                    Console.Clear();

                    break;
               
                case MenuTypes.Exit:
                    await _accountMenu.LogoutAsync(cancellationToken);
                    Console.Clear();

                    return;
                
                default:
                    "Invalid input. Try again.".Print(ConsoleColor.Red);
                    continue;
            }
        }
    }
    
    private static void PrintGreeting()
    {
        ("VERSION 2.0\n" +
         "Welcome to the world best game ----> Rock-Paper-Scissors!\n" +
         "You are given the opportunity to compete with other users in this wonderful game,\n" +
         "or if you don’t have anyone to play, don’t worry,\n" +
         "you can find a random player or just try your skill with a bot.").Print(ConsoleColor.White);
        
        "(c)Ihor Volokhovych & Michael Terekhov".Print(ConsoleColor.Cyan);
    }
}