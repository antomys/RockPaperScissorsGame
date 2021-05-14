using System.Threading.Tasks;
using Server.Bll.Models;

namespace Server.Bll.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(AccountModel accountModel);
        Task<string> LogInAsync(AccountModel accountModel);
        Task<bool> LogOutAsync(string accountToken);
    }
}