using System.Threading.Tasks;
using Client.Models;

namespace Client.Menus;

public interface IAccountMenu
{
    Task<bool> RegisterAsync();
   
    Task<(string token, TokenModel inputAccount)> LoginAsync();
    
    Task<bool> LogoutAsync(string token);
}