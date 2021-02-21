using System.Threading.Tasks;
using Server.Models;

namespace Server.Services
{
    public interface IAccountManager
    {
        Task<Account> LogInAsync(string name, string password);
    }
}