using System;
using System.Net;
using System.Threading.Tasks;
using Client.Menus.Interfaces;
using Client.Models;
using Client.Models.Interfaces;
using Client.Services;
using Client.Services.RequestProcessor;
using Client.Services.RequestProcessor.RequestModels.Impl;
using Newtonsoft.Json;

namespace Client.Menus
{
    internal class LoginMenu : ILoginMenu
    {
        private readonly string _sessionId;
        private readonly string _baseAddress;
        private readonly IRequestPerformer _performer;
        private IAccount _playerAccount;

        public LoginMenu(
            string sessionId, 
            string baseAddress,
            IRequestPerformer requestPerformer)
        {
            _sessionId = sessionId;
            _baseAddress = baseAddress;
            _performer = requestPerformer;
        }

        public async Task<int> LogIn()
        {
            var inputAccount = new Account
            {
                SessionId = _sessionId,
                Login = new StringPlaceholder().BuildNewSpecialDestinationString("Login"),
                Password =
                    new StringPlaceholder(StringDestination.Password).BuildNewSpecialDestinationString("Password", true),
                LastRequest = DateTime.Now
            };
            var options = new RequestOptions
            {
                ContentType = "application/json",
                Body = JsonConvert.SerializeObject(inputAccount),
                Address = _baseAddress + "user/login",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Post,
                Name = "Registration"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
            if (reachedResponse.Code == (int) HttpStatusCode.OK)
            {
                _playerAccount =  inputAccount;
                ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Green);
                Console.ReadKey();
                Console.Clear();
                return 0;
            }
            ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Red);
            return -1;
        }

        public async Task Logout()
        {
            var options = new RequestOptions
            {
                Address = _baseAddress + $"user/logout/{_sessionId}",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Get
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
            if (reachedResponse.Code == 200)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor("Successfully signed out", ConsoleColor.Green);
            }
            else
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Red);
            }
        }
    }
}