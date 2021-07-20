using System.Threading.Tasks;
using Server.Bll.Services.Interfaces;
using Server.Dal.Context;

namespace Server.Bll.Services
{
    public class LongPollingService : ILongPollingService
    {
        private readonly ServerContext _serverContext;

        public LongPollingService(ServerContext serverContext)
        {
            _serverContext = serverContext;
        }

        public async Task<bool> CheckRoomState(int roomId)
        {
            var thisRoom = await _serverContext.Rooms.FindAsync(roomId);
            return thisRoom != null;
        }

        public async Task<bool> CheckRoundState(int roundId)
        {
            var thisRound = await _serverContext.Rounds.FindAsync(roundId);
            return thisRound is {WinnerId: null};
        }
    }
    public interface ILongPollingService
    {
        Task<bool> CheckRoomState(int roomId);
        Task<bool> CheckRoundState(int roundId);
    }
}