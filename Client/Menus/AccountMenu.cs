using System;
using System.Net;
using System.Threading.Tasks;
using Client.Models;
using Client.Services;
using Client.Services.RequestProcessor;
using Client.Services.RequestProcessor.RequestModels.Impl;
using Newtonsoft.Json;

namespace Client.Menus
{
    public class AccountMenu : IAccountMenu
    {
        private readonly IRequestPerformer _performer;

        public AccountMenu(IRequestPerformer performer)
        {
            _performer = performer;
        }

        public async Task<bool> Register()
        {
            TextWrite.Print("\nWe are glad to welcome you in the registration form!\n" +
                "Please enter the required details\n" +
                "to register an account on the platform", ConsoleColor.Magenta);
            var registrationAccount = new Account
            {
                Login = new StringPlaceholder().BuildString("Login"),
                Password =
                    new StringPlaceholder(StringDestination.Password).BuildString("Password"),
                LastRequest = DateTime.Now
            };

            var options = new RequestOptions
            {
                ContentType = "application/json",
                Body = JsonConvert.SerializeObject(registrationAccount),
                Address = "user/register",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Post,
                Name = "Registration"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
            if (reachedResponse.Code == (int) HttpStatusCode.Created)
            {
                
                TextWrite.Print(reachedResponse.Content, ConsoleColor.Green);
                return true;
            }
            
            TextWrite.Print(reachedResponse.Content, ConsoleColor.Red);
            return false;
        }
        
        public async Task<(string sessionId, Account inputAccount)> LogIn()
        {
            var inputAccount = new Account
            {
                Login = new StringPlaceholder().BuildString("Login"),
                Password =
                    new StringPlaceholder(StringDestination.Password).BuildString("Password", true),
                LastRequest = DateTime.Now
            };
            var options = new RequestOptions
            {
                ContentType = "application/json",
                Body = JsonConvert.SerializeObject(inputAccount),
                Address = "user/login",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Post,
                Name = "Login"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
            if (reachedResponse.Code == (int) HttpStatusCode.OK)
            {
                TextWrite.Print(reachedResponse.Content, ConsoleColor.Green);
                
                //returns sessionId
                var sessionId = reachedResponse.Content.Replace("\"","").Trim();
                TextWrite.Print($"Successfully signed in! session id : {sessionId}", ConsoleColor.DarkGreen);
                return (sessionId, inputAccount);
            }

            TextWrite.Print(reachedResponse.Content, ConsoleColor.Red);

            return (null, null);
        }

        public async Task<bool> Logout(string sessionId)
        {
            var options = new RequestOptions
            {
                Address = $"user/logout/{sessionId}",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Get
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
            if (reachedResponse.Code == 200)
            {
                TextWrite.Print("Successfully signed out", ConsoleColor.Green);
                return true;
            }

            TextWrite.Print(reachedResponse.Content, ConsoleColor.Red);

            return false;
        }
    }
}