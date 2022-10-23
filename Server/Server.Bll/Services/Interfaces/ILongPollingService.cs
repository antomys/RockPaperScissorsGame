namespace Server.Bll.Services.Interfaces;

public interface ILongPollingService
{
    Task<long> GetRoomUpdateTicksAsync(string roomId);

    Task<long> GetRoundUpdateTicksAsync(string roundId);
}