using System;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Server.Bll.Exceptions;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;
using OneOf;
using Server.Data.Context;
using Server.Data.Entities;

namespace Server.Bll.Services;

internal sealed class RoundService: IRoundService
{
    private readonly ServerContext _serverContext;

    public RoundService(ServerContext serverContext)
    {
        _serverContext = serverContext ?? throw new ArgumentNullException(nameof(serverContext));
    }
    
    public async Task<OneOf<RoundModel, CustomException>> CreateAsync(string userId, string roomId)
    {
        var playingRoom = await _serverContext.Rooms
            .Include(rooms => rooms.Players)
            .FirstOrDefaultAsync(room => room.Id.Equals(roomId));
        
        if (playingRoom is null)
        {
            return new CustomException(ExceptionTemplates.NotExists(nameof(Room)));
        }

        if (!playingRoom.IsFull)
        {
            return new CustomException(ExceptionTemplates.RoomNotFull);
        }
        
        if (!playingRoom.Players.Any(player => player.AccountId.Equals(userId)))
        {
            return new CustomException(ExceptionTemplates.NotAllowed);
        }

        var updateTime = DateTimeOffset.UtcNow.Ticks;
        var newRound = new Round
        {
            Id = Guid.NewGuid().ToString(),
            RoomId = roomId,
            Room = playingRoom,
            StartTimeTicks = updateTime,
            UpdateTicks = updateTime,
            IsFinished = false
        };

        playingRoom.UpdateTicks = updateTime;
        
        var players = playingRoom.Players;

        newRound.Players = players;

        _serverContext.Rounds.Add(newRound);
        _serverContext.Rooms.Update(playingRoom);

        await _serverContext.SaveChangesAsync();
       
        return newRound.Adapt<RoundModel>();
    }

    [Obsolete(message: "Not used in new version. Please use UpdateRoundAsync")]
    public Task<RoundModel> MakeMoveAsync()
    {
        throw new NotImplementedException();
    }

    public Task<OneOf<RoundModel, CustomException>> UpdateAsync(string userId, RoundModel roundModel)
    {
        throw new NotImplementedException();
    }

    public async Task<OneOf<RoundModel,CustomException>> UpdateAsync(int userId, RoundModel roundModel)
    {
        throw new NotImplementedException();
        // var thisRound = await _serverContext
        //     .Rounds
        //     .Include(x => x.Player)
        //     .ThenInclude(x=>x.Room)
        //     .FirstOrDefaultAsync(x => x.Id == roundModel.Id);
        //     
        // if(thisRound is null)
        // {
        //     return new CustomException(ExceptionTemplates.RoundNotFound(roundModel.Id));
        // }
        //
        // if(thisRound.Player.FirstPlayerId != userId || thisRound.Player.SecondPlayerId != userId)
        // {
        //     return new CustomException(ExceptionTemplates.NotAllowed);
        // }
        //
        // var incomeRound = roundModel.Adapt<Round>();
        // thisRound.FirstPlayerMove = incomeRound.FirstPlayerMove;
        // thisRound.SecondPlayerMove = incomeRound.SecondPlayerMove;
        // thisRound.LastMoveTicks = incomeRound.LastMoveTicks;
        //
        // if (thisRound.FirstPlayerMove != 0 && thisRound.SecondPlayerMove != 0)
        // {
        //     thisRound.IsFinished = true;
        //     thisRound.TimeFinishedTicks = DateTimeOffset.Now.Ticks;
        // }
        //
        // if (!_serverContext.Entry(thisRound).Properties.Any(x => x.IsModified))
        // {
        //     return new CustomException(ExceptionTemplates.NotAllowed);
        // }
        //     
        // _serverContext.Update(thisRound);
        // await _serverContext.SaveChangesAsync();
        //
        // return thisRound.Adapt<RoundModel>();
    }
}