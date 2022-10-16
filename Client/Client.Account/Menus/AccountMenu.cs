using Client.Account.Enums;
using Client.Account.Extensions;
using Client.Account.Services;
using Client.Account.Services.Interfaces;
using RockPaperScissors.Common.Enums;
using RockPaperScissors.Common.Extensions;

namespace Client.Account.Menus;

internal sealed class AccountMenu : IAccountMenu
{
    private readonly IAccountService _accountService;

    public AccountMenu(IAccountService accountService)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            "Account Menu:".Print(ConsoleColor.DarkYellow);
            $"{MenuTypes.SignUp.GetValue()}.\t{MenuTypes.SignUp.GetDisplayName()}".Print(ConsoleColor.DarkYellow);
            $"{MenuTypes.Login.GetValue()}.\t{MenuTypes.Login.GetDisplayName()}".Print(ConsoleColor.DarkYellow);
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
                case MenuTypes.SignUp: 
                    var isRegistered = await RegisterAsync(cancellationToken);

                    if (!isRegistered)
                    {
                        "\nPress any key to back to the start up menu list!".Print(ConsoleColor.Cyan);
                        Console.ReadKey();
                        Console.Clear();
                        
                        continue;
                    }

                    "Trying logging in...".Print(ConsoleColor.White);
                    Console.Clear();

                    return;
                
                case MenuTypes.Login: 
                    var isSuccess = await LoginAsync(cancellationToken);
                    
                    if (isSuccess)
                    {
                        "\nPress any key to back to the start up menu list!".Print(ConsoleColor.Cyan);
                        Console.ReadKey();
                        Console.Clear();

                        return;
                    }

                    continue;

                case MenuTypes.Back:
                    Console.Clear();

                    return;

                default:
                    "Invalid input. Try again.".Print(ConsoleColor.Red);
                    continue;
            }
        }
    }

    public async Task<bool> LogoutAsync(CancellationToken cancellationToken)
    {
        if (!_accountService.IsAuthorized())
        {
            return true;
        }

        var user = _accountService.GetUser();
        
        var response = await _accountService.LogoutAsync(user.SessionId, cancellationToken);
       
        return response.Match(
            _ => 
            {
                "Successfully signed out".Print(ConsoleColor.DarkGreen);

                return true;
            },
            exception =>
            {
                exception.Message.Print(ConsoleColor.Red);

                return false;
            });
    }

    private async Task<bool> RegisterAsync(CancellationToken cancellationToken)
    { 
        ("We are glad to welcome you in the registration form!\n" +
         "Please enter the required details\n" +
         "to register an account on the platform")
            .Print(ConsoleColor.Magenta);

        var login = "Login: ".BuildString(StringDestination.Login);
        var password = "Password: ".BuildString(StringDestination.Password);

        var response = await _accountService.SignUpAsync(login, password, cancellationToken);

        return response.Match(
            _ =>
            {
                "Successfully registered!".Print(ConsoleColor.Green);

                return true;
            },
            exception =>
            {
                exception.Message.Print(ConsoleColor.Red);

                return false;
            });
    }
        
    private async Task<bool> LoginAsync(CancellationToken cancellationToken)
    {
        var login = "Login: ".BuildString(StringDestination.Login);
        var password = "Password: ".BuildString(StringDestination.Password, isNeedConfirmation: true);

        var response = await _accountService.LoginAsync(login, password, cancellationToken);

        return response.Match(
            _ =>
            {
                "Successfully signed in".Print(ConsoleColor.DarkGreen);
                
                return true;
            },
            exception =>
            {
                exception.Message.Print(ConsoleColor.Red);

                return false;
            });
    }
}