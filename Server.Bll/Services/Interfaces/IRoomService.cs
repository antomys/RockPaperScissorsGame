using System.Threading.Tasks;
using OneOf;
using Server.Bll.Exceptions;
using Server.Bll.Models;

namespace Server.Bll.Services.Interfaces
{
    public interface IRoomService
    {
        Task<OneOf<RoomModel, RoomException>> CreateRoom(int userId, bool isPrivate = false);
        Task<OneOf<RoomModel, RoomException>> JoinRoom(int userId, string roomCode);
        Task<OneOf<RoomModel, RoomException>> GetRoom(int roomId);
        Task<int?> UpdateRoom(RoomModel room);
        Task<int?> DeleteRoom(int userId, int roomId);
    }
}