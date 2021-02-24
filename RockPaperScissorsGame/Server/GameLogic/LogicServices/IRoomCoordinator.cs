using Server.GameLogic.Models.Impl;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Server.GameLogic.LogicServices
{
    public interface IRoomCoordinator
    {
        ConcurrentDictionary<string, Room> ActiveRooms { get; }
        Task<Room> CreateRoom(string sessionId, bool isPrivate);
        Task<Room> CreateTrainingRoom(string sessionId);
        Task<Room> UpdateRoom(Room updated);
        Task<Room> UpdatePlayerStatus(string sessionId, bool IsReady);
        Task<bool> DeleteRoom(string roomId);
    }
}
