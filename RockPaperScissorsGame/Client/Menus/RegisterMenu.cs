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
    internal class RegisterMenu : IRegisterMenu
    {
        private readonly string _sessionId;
        private readonly string _baseAddress;
        private IAccount _playerAccount;
        private readonly IRequestPerformer _performer;

        public RegisterMenu(
            string sessionId,
            string baseAddress,
            IAccount playerAccount,
            IRequestPerformer performer)
        {
            _sessionId = sessionId;
            _baseAddress = baseAddress;
            _performer = performer;
            _playerAccount = playerAccount;
        }

        public async Task<int> Registration()
        {
            ColorTextWriterService.PrintLineMessageWithSpecialColor("\nWe are glad to welcome you in the registration form!\n" +
                                                                    "Please enter the required details\n" +
                                                                    "to register an account on the platform", ConsoleColor.Magenta);
            var registrationAccount = new Account
            {
                SessionId = _sessionId,
                Login = new StringPlaceholder().BuildNewSpecialDestinationString("Login"),
                Password =
                    new StringPlaceholder(StringDestination.Password).BuildNewSpecialDestinationString("Password"),
                LastRequest = DateTime.Now
            };
            _playerAccount = registrationAccount;
            
            var options = new RequestOptions
            {
                ContentType = "application/json",
                Body = JsonConvert.SerializeObject(_playerAccount),
                Address = _baseAddress + "user/create",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Post,
                Name = "Registration"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
            if (reachedResponse.Code == (int) HttpStatusCode.Created)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Green);
                return 1;
            }

            ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Red);
            return -1;
        }
    }
}