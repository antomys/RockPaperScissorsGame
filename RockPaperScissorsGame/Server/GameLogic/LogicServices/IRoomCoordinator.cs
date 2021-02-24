using Server.GameLogic.Models.Impl;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Server.GameLogic.LogicServices
{
    public interface IRoomCoordinator
    {
        ConcurrentDictionary<string, Room> ActiveRooms { get; }
        Task<Room> CreateRoom(string sessionId, bool isPrivate);

        Task<Room> JoinPrivateRoom(string sessionId, string roomId);
        Task<Room> CreateTrainingRoom(string sessionId);
        Task<Room> UpdateRoom(Room updated); //maybe to delete in future
        Task<Room> UpdateRoom(string roomId);
        Task<Room> UpdatePlayerStatus(string sessionId, bool isReady);
        Task<bool> DeleteRoom(string roomId);
    }
}
