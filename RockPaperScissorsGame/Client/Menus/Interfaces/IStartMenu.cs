using System.Threading;
using System.Threading.Tasks;

namespace Client.Menus.Interfaces
{
    internal interface IStartMenu
    {
        Task Start(CancellationToken token);
    }
}