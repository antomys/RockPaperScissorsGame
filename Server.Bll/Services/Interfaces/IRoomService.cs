using System;
using System.Threading.Tasks;
using OneOf;
using Server.Bll.Exceptions;
using Server.Bll.Models;

namespace Server.Bll.Services.Interfaces
{
    public interface IRoomService
    {
        Task<OneOf<RoomModel, CustomException>> CreateRoom(int userId, bool isPrivate = false);
        Task<OneOf<RoomModel, CustomException>> JoinRoom(int userId, bool isPrivate, string roomCode);
        Task<OneOf<RoomModel, CustomException>> GetRoom(int roomId);
        Task<int?> UpdateRoom(RoomModel room);
        Task<int?> DeleteRoom(int userId, int roomId);
    }
}