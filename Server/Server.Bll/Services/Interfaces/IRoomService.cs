using OneOf;
using RockPaperScissors.Common;
using Server.Bll.Models;

namespace Server.Bll.Services.Interfaces;

public interface IRoomService
{
    Task<OneOf<RoomModel, CustomException>> CreateAsync(string userId, bool isPrivate = false, bool isTraining = false);

    Task<int> RemoveRangeAsync(TimeSpan roomOutDate, TimeSpan roundOutDate);

    Task<OneOf<RoomModel, CustomException>> JoinAsync(string userId, string? roomCode = null);

    Task<OneOf<RoomModel, CustomException>> GetAsync(string roomId);

    Task<long> GetUpdateTicksAsync(string roomId);

    Task<OneOf<RoomModel, CustomException>> ChangePlayerStatusAsync(string userId, string roomId, bool newStatus);

    Task<OneOf<int, CustomException>> DeleteAsync(string userId, string roomId);
}