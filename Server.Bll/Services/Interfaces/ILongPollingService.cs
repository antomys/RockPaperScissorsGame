using System.Threading.Tasks;

namespace Server.Bll.Services.Interfaces;

public interface ILongPollingService
{
    Task<bool> CheckRoomState(int roomId);
    Task<bool> CheckRoundState(int roundId);
}