using System.Collections.Concurrent;
using System.Threading.Tasks;
using Server.Game.Models;
using Server.Models.Interfaces;

namespace Server.Game.Services
{
    public interface IRoomManager
    {
        ConcurrentDictionary<string, Room> ActiveRooms { get; }
        Task<Room> CreateRoom(string sessionId, bool isPrivate);

        Task<Room> CreateTrainingRoom(string sessionId);

        Task<Room> JoinPrivateRoom(string sessionId, string roomId);

        Task<Room> UpdatePlayerState(string sessionId, bool state);

        Task<Room> UpdateRoom(string login);

        Task<bool> DeleteRoom(string roomId);
    }
}