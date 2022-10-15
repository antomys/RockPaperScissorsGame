using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Account.Extensions;
using Client.StartMenu.Extensions;
using Client.Statistics.Extensions;

namespace Client.Host;

internal static class Program
{
    private static async Task<int> Main()
    {
        var cancellationTokenSource = new CancellationTokenSource();

        try
        {
            var baseAddress = "http://localhost:5000";
            var client = RockPaperScissors.Common.Client.Client.Create(baseAddress);
            var accountService = client.CreateAccountService();
            var statisticsService = client.CreateStatisticsService();
            var healthCheckService = client.CreateHealthCheckService(cancellationTokenSource);

            var accountMenu = accountService.CreateAccountMenu();
            var statisticsMenu = statisticsService.CreateStatisticsMenu(accountService);
            var startMenu = statisticsMenu.CreateStartMenu(accountMenu, healthCheckService);

            await startMenu.PrintAsync(cancellationTokenSource.Token);

            return 0;
        }
        catch (TaskCanceledException)
        {
            return -1;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine("Unknown error occured. Closing.");
            
            return -1;
        }
    }
}