using Client.Account.Services;
using Client.Statistics.EnumExtensions;
using Client.Statistics.Enums;
using Client.Statistics.Services;
using RockPaperScissors.Common.Extensions;
using RockPaperScissors.Common.Responses;

namespace Client.Statistics.Menus;

internal sealed class StatisticsMenu: IStatisticsMenu
{
    private readonly IStatisticsService _statisticsService;
    private readonly IAccountService _accountService;

    public StatisticsMenu(IStatisticsService statisticsService, IAccountService accountService)
    {
        _statisticsService = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    { 
        while (!cancellationToken.IsCancellationRequested)
        {
            "Statistics Menu:".Print(ConsoleColor.DarkYellow);
            $"{MenuTypes.All.GetValue()}.\t{MenuTypes.All.GetDisplayName()}".Print(ConsoleColor.DarkYellow);
            $"{MenuTypes.Personal.GetValue()}.\t{MenuTypes.Personal.GetDisplayName()}".Print(ConsoleColor.DarkYellow);
            $"{MenuTypes.Back.GetValue()}.\t{MenuTypes.Back.GetDisplayName()}".Print(ConsoleColor.DarkYellow);

            "\nPlease select an item from the list".Print(ConsoleColor.Green);

            Console.Write("Select -> ");
            
            var menuType = Console.ReadLine().TryGetMenuType();
            
            if (menuType is MenuTypes.Unknown)
            {
                "Invalid input. Try again.".Print(ConsoleColor.Red);
                
                continue;
            }
             
            switch (menuType)
            {
                case MenuTypes.All:
                    await PrintAllStatisticsAsync(cancellationToken);
                     
                    break;
                 
                case MenuTypes.Personal:
                    await PrintPersonalStatisticsAsync(cancellationToken);
                    
                    break;
                
                case MenuTypes.Back:
                    return;
                 
                default:
                    "Invalid input. Try again.".Print(ConsoleColor.Red);
                    continue;
            }
        }
    }

    private async Task PrintAllStatisticsAsync(CancellationToken cancellationToken)
    {
        var allStatistics = await _statisticsService.GetAllAsync(cancellationToken);

        if (allStatistics.IsT0)
        {
            PrintAllStatistics(allStatistics.AsT0);

            return;
        }
        
        allStatistics.AsT1.Message.Print(ConsoleColor.Red);
    }

    private async Task PrintPersonalStatisticsAsync(CancellationToken cancellationToken)
    {
        if (!_accountService.IsAuthorized())
        {
            "User in not logged in.".Print(ConsoleColor.Red);
            
            return;
        }
        
        var user = _accountService.GetUser();
        var personalStatistics = await _statisticsService.GetPersonalAsync(user.SessionId, cancellationToken);
        
        if (personalStatistics.IsT0)
        {
            PrintStatistics(personalStatistics.AsT0, user.Login!);

            return;
        }
        
        personalStatistics.AsT1.Message.Print(ConsoleColor.Red);
    }
    
    private static void PrintAllStatistics(AllStatisticsResponse[] allStatistics)
    {
        var statisticsSpan = allStatistics.AsSpan();

        for (var index = 0; index < statisticsSpan.Length; index++)
        {
            var color = GetColor(index);
            $"{index + 1}. User: {statisticsSpan[index].Login}; Score: {statisticsSpan[index].Score}".Print(color);
        }
    }
    
    private static void PrintStatistics(PersonalStatisticsResponse allStatistics, string userName)
    {
        $"Here is your statistics, {userName}:".Print(ConsoleColor.White);
        
        "Main statistics: ".Print(ConsoleColor.DarkYellow);
        
        $"\t{nameof(allStatistics.Wins)}: {allStatistics.Wins}".Print(ConsoleColor.Green);
        $"\t{nameof(allStatistics.Draws)}: {allStatistics.Draws}".Print(ConsoleColor.Yellow);
        $"\t{nameof(allStatistics.Loss)}: {allStatistics.Loss}".Print(ConsoleColor.Red);
        $"\t{nameof(allStatistics.Score)}: {allStatistics.Score}".Print(ConsoleColor.White);
        
        "Other statistics: ".Print(ConsoleColor.DarkYellow);
        
        $"\t{allStatistics.TimeSpent} spent time playing".Print(ConsoleColor.White);
        $"\t{allStatistics.UsedPaper} times used 'Paper'".Print(ConsoleColor.White);
        $"\t{allStatistics.UsedRock} times used 'Rock'".Print(ConsoleColor.White);
        $"\t{allStatistics.UsedScissors} times used 'Scissors'".Print(ConsoleColor.White);
        $"\t{allStatistics.WinLossRatio}% Win/Loss ratio".Print(GetColor(allStatistics.WinLossRatio));
    }

    private static ConsoleColor GetColor(int index)
    {
        return index switch
        {
            < 3 => ConsoleColor.Green,
            < 6 => ConsoleColor.Yellow,
            _ => ConsoleColor.White
        };
    }
    
    private static ConsoleColor GetColor(double index)
    {
        return index switch
        {
            > 75d => ConsoleColor.DarkGreen,
            > 50d => ConsoleColor.Green,
            > 25d => ConsoleColor.Yellow,
            > 10d => ConsoleColor.Red,
            _ => ConsoleColor.DarkRed
        };
    }
}