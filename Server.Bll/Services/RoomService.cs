using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Server.Bll.Exceptions;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;
using Server.Data.Context;

namespace Server.Bll.Services;

internal sealed class RoomService : IRoomService
{
    private readonly ServerContext _repository;
    private readonly IRoundService _roundService;

    public RoomService(
        ServerContext repository, 
        IRoundService roundService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _roundService = roundService ?? throw new ArgumentNullException(nameof(roundService));
    }

    public async Task<OneOf<RoomModel, CustomException>> 
        CreateAsync(string userId, bool isPrivate = false, bool isTraining = false)
    {
        throw new NotImplementedException();
        // throw new NotImplementedException();
        // // var doesRoomExist = await _repository.RoomPlayersEnumerable
        // //     .AnyAsync(roomPlayers => roomPlayers.FirstPlayerId == userId 
        // //                               || roomPlayers.SecondPlayerId == userId);
        // //
        // // if (doesRoomExist)
        // // {
        // //     return new CustomException(ExceptionTemplates.TwinkRoom);
        // // }
        // //     
        // // var room = new Room
        // // {
        // //     IsPrivate = isPrivate,
        // //     Code = Guid.NewGuid().ToString("n")[..8],
        // //     IsFull = false,
        // //     CreationTimeTicks = DateTimeOffset.Now.Ticks,
        // //     Player = new Player
        // //     {
        // //         FirstPlayerId = userId,
        // //         Count = 1
        // //     }
        // // }; 
        // //
        // // // await _rooms.AddAsync(room);
        // // // await _repository.SaveChangesAsync();
        // //     
        // // // var newRoomPlayers = new RoomPlayers
        // // // {
        // // //     RoomId = room.Id,
        // // //     FirstPlayerId = userId,
        // // //     PlayersCount = 1
        // // // };
        // // //     
        // // if (isTraining)
        // // {
        // //     room.Player.SecondPlayerId = 0;
        // // }
        // //
        // // _repository.Add(room);
        // //
        // // // room.RoomPlayers = newRoomPlayers;
        // // // _rooms.Update(room);
        // // //     
        // // await _repository.SaveChangesAsync();
        // //     
        // // return room.Adapt<RoomModel>();
    }
        
    public async Task<OneOf<RoomModel, CustomException>> JoinAsync(int userId, bool isPrivate, string roomCode)
    {
        throw new NotImplementedException();
        // var thisRoom = isPrivate 
        //     ? await _rooms
        //         .Include(room => room.Player)
        //         .Where(room => !room.IsFull)
        //         .FirstOrDefaultAsync()
        //     : await _rooms
        //         .Include(room=>room.Player).
        //         FirstOrDefaultAsync(room => room.Code == roomCode);
        //
        // if (thisRoom is null)
        // {
        //     return new CustomException(ExceptionTemplates.RoomNotExists);
        // }
        //
        // if (thisRoom.Player.FirstPlayerId == userId || thisRoom.Player.SecondPlayerId == userId)
        // {
        //     return new CustomException(ExceptionTemplates.AlreadyInRoom);
        // }
        //     
        // if (thisRoom.Player.FirstPlayerId is not 0 && thisRoom.Player.SecondPlayerId is not 0)
        // {
        //     return new CustomException(ExceptionTemplates.RoomFull);
        // }
        //     
        // if (thisRoom.Player.FirstPlayerId is not 0)
        // {
        //     thisRoom.Player.FirstPlayerId = userId;
        //     _rooms.Update(thisRoom);
        //
        //     await _repository.SaveChangesAsync();
        //         
        //     return thisRoom.Adapt<RoomModel>();
        // }
        //
        // if (thisRoom.Player.SecondPlayerId is 0)
        // {
        //     return new CustomException(ExceptionTemplates.Unknown);
        // }
        //     
        // thisRoom.Player.SecondPlayerId = userId;
        //
        // if (thisRoom.Player.FirstPlayerId is not 0 && thisRoom.Player.SecondPlayerId is not 0)
        // {
        //     await _roundService.CreateAsync(userId, thisRoom.Id);
        // }
        //     
        // _rooms.Update(thisRoom);
        // await _repository.SaveChangesAsync();
        //
        // return thisRoom.Adapt<RoomModel>();
    }

    public async Task<OneOf<RoomModel, CustomException>> GetAsync(int roomId)
    {
        throw new NotImplementedException();
        // var room = await _rooms.FindAsync(roomId);
        //
        // return room is not null 
        //     ? room.Adapt<RoomModel>() 
        //     : new CustomException(ExceptionTemplates.RoomNotExists);
    }

    public Task<OneOf<RoomModel, CustomException>> GetAsync(string roomId)
    {
        throw new NotImplementedException();
    }

    public async Task<int?> UpdateAsync(RoomModel room)
    {
        throw new NotImplementedException();
        // var thisRoom = await _rooms.FindAsync(room.Id);
        //     
        // if (thisRoom is null)
        // {
        //     return StatusCodes.Status400BadRequest;
        // }
        //    
        // var updatedRoom = room.Adapt<Room>();
        //
        // thisRoom.IsFull = updatedRoom.IsFull;
        // thisRoom.IsPrivate = updatedRoom.IsPrivate;
        // thisRoom.RoundId = updatedRoom.RoundId;
        //
        // if (!_repository.Entry(thisRoom).Properties.Any(x => x.IsModified))
        // {
        //     return StatusCodes.Status400BadRequest;
        // }
        //     
        // _repository.Update(thisRoom);
        //     
        // return StatusCodes.Status200OK;
    }

    public Task<int?> DeleteAsync(string userId, string roomId)
    {
        throw new NotImplementedException();
    }

    public async Task<int?> DeleteAsync(int userId, int roomId)
    {
        throw new NotImplementedException();
        // var thisRoom = await _rooms
        //     .Include(room=>room.Player)
        //     .FirstOrDefaultAsync(room => room.Id == roomId);
        //     
        // if (thisRoom is null)
        // {
        //     return StatusCodes.Status400BadRequest;
        // }
        //     
        // if (thisRoom.Player.FirstPlayerId != userId)
        // {
        //     return StatusCodes.Status400BadRequest;
        // }
        //    
        // if (thisRoom.Player.SecondPlayerId != userId)
        // {
        //     return StatusCodes.Status400BadRequest;
        // }
        //     
        // _rooms.Remove(thisRoom);
        //
        // await _repository.SaveChangesAsync();
        //     
        // return StatusCodes.Status200OK;
    }

    public async Task<int> RemoveRangeAsync(TimeSpan roomOutDate, TimeSpan roundOutDate)
    {
        var currentDate = DateTimeOffset.UtcNow.Ticks;
           
        var rooms = await _repository.Rooms
                .Include(room => room.Round)
                .Where(room => room.CreationTimeTicks + roomOutDate.Ticks < currentDate && room.Round == null)
                .ToArrayAsync();

        var roomLength = rooms.Length;
        
        var allRounds = await _repository.Rounds
            .Where(round => round.FinishTimeTicks + roundOutDate.Ticks < currentDate)
            .ToArrayAsync();

        var roundsLength = allRounds.Length;
        
        if (roomLength is not 0)
        {
            _repository.Rooms.RemoveRange(rooms);   
        }

        if (roundsLength is not 0)
        {
            _repository.Rounds.RemoveRange(allRounds);   
        }

        if (roundsLength is not 0 && roomLength is not 0)
        {
            await _repository.SaveChangesAsync();   
        }

        return roomLength + roundsLength;
    }

    public Task<OneOf<RoomModel, CustomException>> JoinAsync(string userId, bool isPrivate, string? roomCode = default)
    {
        throw new NotImplementedException();
    }
}