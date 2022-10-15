using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Account.Extensions;
using Client.StartMenu.Extensions;
using Client.Statistics.Extensions;

namespace Client.Host;

internal static class Program
{
    // private static async Task<int> MainV1()
    // {
    //     try
    //     {
    //         var client = new HttpClient {BaseAddress = new Uri("http://localhost:5000/api/v1/")};
    //         var clientHandler = new HttpClientHandler();
    //         var requestHandler = new RequestHandler(client, clientHandler);
    //         var requestPerformer = new RequestPerformer(requestHandler);
    //             
    //         var startMenu = new StartMenu(requestPerformer);
    //         return await startMenu.StartAsync();
    //     }
    //     catch (Exception) //todo : do this need a message?
    //     {
    //         Console.WriteLine("Unknown error occured. Crash.");
    //         return -1;
    //     }
    // }
    
    private static async Task<int> Main()
    {
        var cancellationToken = new CancellationTokenSource();
        try
        {
            var client = RockPaperScissors.Common.Client.Client.Create("http://localhost:5000");
            var accountService = client.CreateAccountService();
            var statisticsService = client.CreateStatisticsService();

            var accountMenu = accountService.CreateAccountMenu();
            var statisticsMenu = statisticsService.CreateStatisticsMenu(accountService);
            var startMenu = statisticsMenu.CreateStartMenu(accountMenu);

            await startMenu.PrintAsync(cancellationToken.Token);
            
            return 0;
        }
        catch (Exception exception) //todo : do we need a message?
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine("Unknown error occured. Closing.");
            
            return -1;
        }
    }
}