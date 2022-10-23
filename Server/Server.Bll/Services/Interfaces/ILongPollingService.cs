namespace Server.Bll.Services.Interfaces;

public interface ILongPollingService
{
    [Obsolete]
    Task<bool> CheckRoomState(string roomId);

    Task<long> GetRoomUpdateTicksAsync(string roomId);

    [Obsolete]
    Task<bool> CheckRoundState(string roundId);

    Task<long> GetRoundUpdateTicksAsync(string roundId);
}