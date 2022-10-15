using Client.Account.Menus;
using Client.Account.Services;
using RockPaperScissors.Common.Client;

namespace Client.Account.Extensions;

public static class ServiceCollectionExtensions
{
    public static IAccountService CreateAccountService(this IClient client)
    {
        return new AccountService(client);
    }
    
    public static IAccountMenu CreateAccountMenu(this IAccountService accountService)
    {
        return new AccountMenu(accountService);
    }
}