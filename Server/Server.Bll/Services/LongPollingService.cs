using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Bll.Services.Interfaces;
using Server.Data.Context;

namespace Server.Bll.Services;

internal sealed class LongPollingService : ILongPollingService
{
    private readonly ServerContext _serverContext;

    public LongPollingService(ServerContext serverContext)
    {
        _serverContext = serverContext;
    }

    public Task<bool> CheckRoomState(string roomId)
    {
        return _serverContext.Rooms
            .AnyAsync(room => room.Id.Equals(roomId));
    }
    
    public async Task<long> GetRoomUpdateTicksAsync(string roomId)
    {
        var room = await _serverContext.Rooms
            .FindAsync(roomId);

        if (room is null)
        {
            return -1;
        }

        return room.UpdateTicks;
    }

    public async Task<bool> CheckRoundState(string roundId)
    {
        var round = await _serverContext.Rounds
            .FirstOrDefaultAsync(rounds => rounds.Id.Equals(roundId));

        return round?.IsFinished ?? false;
    }
    
    public async Task<long> GetRoundUpdateTicksAsync(string roundId)
    {
        var round = await _serverContext.Rounds
            .FirstOrDefaultAsync(rounds => rounds.Id.Equals(roundId));

        return round?.UpdateTicks ?? -1;
    }
}