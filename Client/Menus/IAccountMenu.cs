using System.Threading.Tasks;

namespace Client.Menus
{
    public interface IAccountMenu
    {
        Task<bool> Register();
        Task<(string sessionId, Models.Account inputAccount)> LogIn();
        Task<bool> Logout(string sessionId);
    }
}