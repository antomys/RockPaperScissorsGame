using System.Collections.Concurrent;
using System.Threading.Tasks;
using RockPaperScissors.Models;
using Server.Models;

namespace Server.Services.Interfaces
{
    public interface IAccountManager
    {
        ConcurrentDictionary<string, Account> AccountsActive { get; set; }
        Task<Account> LogInAsync(AccountDto accountDto);
        Task<bool> LogOutAsync(string sessionId);
    }
}