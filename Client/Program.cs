using Client.Services.RequestProcessor.Impl;
using System;
using System.Threading.Tasks;

namespace Client
{
    internal static class Program
    {
        private static async Task<int> Main()
        {
            try
            {
                var emulator = new ClientAppEmulator(new RequestPerformer());
                return await emulator.StartAsync();
            }
            catch (Exception) //todo : do this need a message?
            {
                return -1;
            }
        }
    }
}