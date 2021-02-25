using System.Threading.Tasks;

namespace Client.Menus.Interfaces
{
    internal interface IRoomMenu
    {
        Task JoinPublicRoom();
        Task JoinPrivateRoom();
        Task CreateRoom();
        Task ChangePlayerStatus();
        Task StartRoomMenu();
        Task RecurrentlyUpdateRoom();
    }
}