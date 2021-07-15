using Client.Services.RequestProcessor.Impl;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Client.Menus;

namespace Client
{
    internal static class Program
    {
        private static async Task<int> Main()
        {
            try
            {
                var client = new HttpClient {BaseAddress = new Uri("http://localhost:5000/api/v1/")};
                var clientHandler = new HttpClientHandler();
                var requestHandler = new RequestHandler(client, clientHandler);
                var requestPerformer = new RequestPerformer(requestHandler);
                
                var startMenu = new StartMenu(requestPerformer);
                return await startMenu.StartAsync();
            }
            catch (Exception) //todo : do this need a message?
            {
                Console.WriteLine("Unknown error occured. Crash.");
                return -1;
            }
        }
    }
}