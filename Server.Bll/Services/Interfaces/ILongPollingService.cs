using System.Threading.Tasks;

namespace Server.Bll.Services.Interfaces;

public interface ILongPollingService
{
    Task<bool> CheckRoomState(string roomId);
    
    Task<bool> CheckRoundState(string roundId);
}