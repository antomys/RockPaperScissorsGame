using System.Collections.Concurrent;
using System.Threading.Tasks;
using Server.Contracts;
using Server.Exceptions.LogIn;
using Server.Models;

namespace Server.Services.Interfaces
{
    public interface IAccountManager
    {
        Task<bool> RegisterAsync(string login, string password);
        Task<string> LogInAsync(AccountDto accountDto);
        
        Task<bool> LogOutAsync(string sessionId);
        
        Task<bool> IsActiveAsync(string sessionId);

        Account GetActiveAccountBySessionId(string sessionId);
    }
}