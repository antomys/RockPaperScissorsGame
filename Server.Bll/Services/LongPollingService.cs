using System;
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
        return _serverContext.Rooms.AnyAsync(room => room.Id.Equals(roomId));
    }

    public async Task<bool> CheckRoundState(string roundId)
    {
        var thisRound = await _serverContext.Rounds.FirstOrDefaultAsync(rounds => rounds.Id.Equals(roundId));

        return thisRound?.IsFinished ?? false;
    }
}