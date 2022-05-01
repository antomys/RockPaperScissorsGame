using System.Threading.Tasks;
using Server.Bll.Services.Interfaces;
using Server.Dal.Context;

namespace Server.Bll.Services;

internal sealed class LongPollingService : ILongPollingService
{
    private readonly ServerContext _serverContext;

    public LongPollingService(ServerContext serverContext)
    {
        _serverContext = serverContext;
    }

    public async Task<bool> CheckRoomState(int roomId)
    {
        var thisRoom = await _serverContext.Rooms.FindAsync(roomId.ToString());
            
        return thisRoom is not null;
    }

    public async Task<bool> CheckRoundState(int roundId)
    {
        var thisRound = await _serverContext.Rounds.FindAsync(roundId.ToString());
            
        return thisRound is {WinnerId: null};
    }
}