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
                var client = new HttpClient {BaseAddress = new Uri("http://localhost:5000/")};
                var requestHandler = new RequestHandler(client);
                var performer = new RequestPerformer(requestHandler);
                //var emulator = new ClientAppEmulator(new RequestPerformer());

                var startMenu = new StartMenu(performer);
                return await startMenu.StartAsync();
            }
            catch (Exception) //todo : do this need a message?
            {
                return -1;
            }
        }
    }
}