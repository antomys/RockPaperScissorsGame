using System.Threading.Tasks;

namespace Client.Menus.Interfaces
{
    internal interface ILoginMenu
    {
        Task<int> LogIn();
        Task Logout();
    }
}