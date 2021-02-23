using System.Threading.Tasks;
using Server.Game.Models;
using Server.Models.Interfaces;

namespace Server.Game.Services
{
    public interface IRoomManager
    {
        Task<Room> CreateRoom(IAccount account, string sessionId, bool isPrivate);

        Task<Room> JoinPrivateRoom(IAccount account, string sessionId, string roomId);

        Task<Room> UpdatePlayerState(IAccount account, bool state);

        Task<Room> UpdateRoom(string login);
    }
}