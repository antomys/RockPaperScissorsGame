using System;
using System.Threading.Tasks;
using OneOf;
using Server.Bll.Exceptions;
using Server.Bll.Models;

namespace Server.Bll.Services.Interfaces;

public interface IRoomService
{
    Task<OneOf<RoomModel, CustomException>> CreateAsync(int userId, bool isPrivate = false, bool isTraining = false);
    
    Task<int> RemoveEntityRangeByDate(TimeSpan roomOutDate, TimeSpan roundOutDate);
    
    Task<OneOf<RoomModel, CustomException>> JoinAsync(int userId, bool isPrivate, string roomCode);
    
    Task<OneOf<RoomModel, CustomException>> GetAsync(int roomId);
    
    Task<int?> UpdateRoom(RoomModel room);
    
    Task<int?> DeleteAsync(int userId, int roomId);
}