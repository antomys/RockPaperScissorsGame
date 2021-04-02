using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    public interface IAccountManager
    {
        Task<bool> RegisterAsync(string login, string password);
        Task<string> LogInAsync(string login, string password);
        Task<int> LogOutAsync(string sessionId);
    }
}