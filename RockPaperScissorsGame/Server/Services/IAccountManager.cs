using System.Threading.Tasks;
using RockPaperScissors.Models;
using Server.Models;

namespace Server.Services
{
    public interface IAccountManager
    {
        Task<Account> LogInAsync(AccountDto accountDto);
    }
}