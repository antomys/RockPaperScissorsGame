using System.Threading.Tasks;
using Server.Models;

namespace Server.Services.Interfaces
{
    public interface IRoomManager
    {

        Task<Room> CreateRoom(string sessionId, bool isPrivate);
        Task<Room> JoinRoom(string sessionId, string roomType, string roomId);

        /*Task<Room> JoinPrivateRoom(string sessionId, string roomId);
        Task<Room> JoinPublicRoom(string sessionId);
        Task<Room> UpdateRoom(string roomId);
        Task<Room> UpdatePlayerStatus(string sessionId, bool isReady);
        Task<bool> DeleteRoom(string roomId);*/
    }
}
