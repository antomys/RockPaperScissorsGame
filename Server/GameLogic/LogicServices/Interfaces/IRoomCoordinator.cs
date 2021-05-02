using System.Collections.Concurrent;
using System.Threading.Tasks;
using Server.GameLogic.Models;

namespace Server.GameLogic.LogicServices.Interfaces
{
    public interface IRoomCoordinator
    {
        
        /// <summary>
        /// List of active rooms. Made for monitoring
        /// </summary>
        ConcurrentDictionary<string, Room> ActiveRooms { get; }
        
        /// <summary>
        /// Method to create a room
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="isPrivate"></param>
        /// <returns></returns>
        Task<Room> CreateRoom(string sessionId, bool isPrivate);
        
        /// <summary>
        /// Method to join a private room by id
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        Task<Room> JoinPrivateRoom(string sessionId, string roomId);
        
        /// <summary>
        /// Method to join random public room
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Task<Room> JoinPublicRoom(string sessionId);
        
        /// <summary>
        /// Method to play with Bot
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Task<Room> CreateTrainingRoom(string sessionId);
        
        /// <summary>
        /// OBSOLETE method to update Rooms
        /// </summary>
        /// <param name="updated"></param>
        /// <returns></returns>
        Task<Room> UpdateRoom(Room updated);
        
        /// <summary>
        /// Method to update Rooms
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        Task<Room> UpdateRoom(string roomId);
        
        /// <summary>
        /// Updates player status
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="isReady"></param>
        /// <returns></returns>
        Task<Room> UpdatePlayerStatus(string sessionId, bool isReady);
        
        /// <summary>
        /// Deletes room
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        Task<bool> DeleteRoom(string roomId);
    }
}
