using System;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Server.Bll.Exceptions;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;
using Server.Dal.Context;
using OneOf;
using Server.Dal.Entities;

namespace Server.Bll.Services;

internal sealed class RoundService : IRoundService
{
    private readonly ServerContext _serverContext;

    public RoundService(ServerContext serverContext)
    {
        _serverContext = serverContext;
    }
    public async Task<OneOf<RoundModel,CustomException>> CreateAsync(int userId, int roomId)
    {
        var foundRoom = await _serverContext
            .Rooms
            .Include(x => x.RoomPlayers)
            .FirstOrDefaultAsync(x => x.Id == roomId);
        
        if (foundRoom is null)
        {
            return new CustomException(ExceptionTemplates.RoomNotExists);
        }
        
        if (foundRoom.RoomPlayers.FirstPlayerId != userId)
        {
            if(foundRoom.RoomPlayers.SecondPlayerId != userId)
            {
                return new CustomException(ExceptionTemplates.NotAllowed);
            }
                
        }
        if (foundRoom.RoomPlayers.SecondPlayerId != userId)
        {
            if(foundRoom.RoomPlayers.FirstPlayerId != userId)
            {
                return new CustomException(ExceptionTemplates.NotAllowed);
            }
        }
                
        if (!foundRoom.IsFull)
        {
            return new CustomException(ExceptionTemplates.RoomNotFull);
        }

        var newRound = new Round
        {
            RoomPlayersId = foundRoom.RoomPlayers.Id,
            FirstPlayerMove = 0,
            SecondPlayerMove = 0,
            LastMoveTicks = DateTimeOffset.Now.Ticks,
            TimeFinishedTicks = 0,
            IsFinished = false
        };

        await _serverContext.AddAsync(newRound);

        foundRoom.RoundId = newRound.Id;
        await _serverContext.SaveChangesAsync();

        return newRound.Adapt<RoundModel>();
    }

    [Obsolete(message: "Not used in new version. Please use UpdateRoundAsync")]
    public Task<RoundModel> MakeMoveAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<OneOf<RoundModel,CustomException>> UpdateAsync(int userId, RoundModel roundModel)
    {
        var thisRound = await _serverContext
            .Rounds
            .Include(x => x.RoomPlayers)
            .ThenInclude(x=>x.Room)
            .FirstOrDefaultAsync(x => x.Id == roundModel.Id);
            
        if(thisRound is null)
        {
            return new CustomException(ExceptionTemplates.RoundNotFound(roundModel.Id));
        }
        
        if(thisRound.RoomPlayers.FirstPlayerId != userId || thisRound.RoomPlayers.SecondPlayerId != userId)
        {
            return new CustomException(ExceptionTemplates.NotAllowed);
        }

        var incomeRound = roundModel.Adapt<Round>();
        thisRound.FirstPlayerMove = incomeRound.FirstPlayerMove;
        thisRound.SecondPlayerMove = incomeRound.SecondPlayerMove;
        thisRound.LastMoveTicks = incomeRound.LastMoveTicks;

        if (thisRound.FirstPlayerMove != 0 && thisRound.SecondPlayerMove != 0)
        {
            thisRound.IsFinished = true;
            thisRound.TimeFinishedTicks = DateTimeOffset.Now.Ticks;
        }

        if (!_serverContext.Entry(thisRound).Properties.Any(x => x.IsModified))
        {
            return new CustomException(ExceptionTemplates.NotAllowed);
        }
            
        _serverContext.Update(thisRound);
        await _serverContext.SaveChangesAsync();
        
        return thisRound.Adapt<RoundModel>();
    }
}