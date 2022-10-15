using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Extensions;
using Client.Models;
using Client.Services;
using Client.Services.RequestProcessor;
using Client.Services.RequestProcessor.RequestModels;
using Client.Services.RequestProcessor.RequestModels.Impl;
using Newtonsoft.Json;

namespace Client.Menus;

public class AccountMenu : IAccountMenu
{
    private readonly IRequestPerformer _performer;

    public AccountMenu(IRequestPerformer performer)
    {
        _performer = performer;
    }

    public async Task<bool> RegisterAsync()
    {
        TextWrite.Print("\nWe are glad to welcome you in the registration form!\n" +
                        "Please enter the required details\n" +
                        "to register an account on the platform", ConsoleColor.Magenta);
        var registrationAccount = new Account
        {
            Login = new StringPlaceholder().BuildString("Login"),
            Password =
                new StringPlaceholder(StringDestination.Password).BuildString("Password")
        };

        var options = new RequestOptions
        {
            ContentType = "application/json",
            Body = JsonConvert.SerializeObject(registrationAccount),
            Address = "account/register",
            IsValid = true,
            Method = RequestMethod.Post,
            Name = "Registration"
        };
        var reachedResponse = await _performer.PerformRequestAsync(options);

        if (reachedResponse.TryParseJson<ErrorModel>(out var errorModel))
        {
            TextWrite.Print(errorModel.Message, ConsoleColor.Red);
            return false;
        }

        TextWrite.Print("Successfully registered!", ConsoleColor.Green);
        return true;
    }
        
    public async Task<(string token, TokenModel inputAccount)> LoginAsync()
    {
        var inputAccount = new Account
        {
            Login = new StringPlaceholder().BuildString("Login"),
            Password =
                new StringPlaceholder(StringDestination.Password).BuildString("Password", true)
        };
        var options = new RequestOptions
        {
            ContentType = "application/json",
            Body = JsonConvert.SerializeObject(inputAccount),
            Address = "account/login",
            IsValid = true,
            Method = RequestMethod.Post,
            Name = "Login"
        };
        var reachedResponse = await _performer.PerformRequestAsync(options);

        if (reachedResponse.TryParseJson<TokenModel>(out var tokenModel))
        {
            TextWrite.Print($"Successfully signed in.", ConsoleColor.DarkGreen);
            return (tokenModel.Token, tokenModel);
        }

        var error = JsonConvert.DeserializeObject<ErrorModel>(reachedResponse.Content);
        if(error is null) return (null, null);
            
        TextWrite.Print(error.Message, ConsoleColor.Red);
        return (null, null);
    }

    public async Task<bool> LogoutAsync(string token)
    {
        var options = new RequestOptions
        {
            Headers = new Dictionary<string, string>{{"Authorization",token}},
            Address = $"user/logout/{token}",
            IsValid = true,
            Method = RequestMethod.Get
        };
        await _performer.PerformRequestAsync(options);
        TextWrite.Print("Successfully signed out", ConsoleColor.Green);
        return true;
    }
}