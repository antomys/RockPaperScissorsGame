using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync();
        Task<string> LogInAsync();
        Task<bool> LogOutAsync();
    }
}