using System;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Server.Bll.Exceptions;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;
using Server.Dal.Context;
using Server.Dal.Entities;

namespace Server.Bll.Services;

internal sealed class RoomService : IRoomService
{
    private readonly DbSet<Room> _rooms;
    private readonly ServerContext _repository;
    private readonly IRoundService _roundService;

    public RoomService(ServerContext repository, 
        IRoundService roundService)
    {
        _repository = repository;
        _roundService = roundService;
        _rooms = _repository.Rooms;
    }

    public async Task<OneOf<RoomModel, CustomException>> 
        CreateAsync(int userId, bool isPrivate = false, bool isTraining = false)
    {
        throw new NotImplementedException();
        // var doesRoomExist = await _repository.RoomPlayersEnumerable
        //     .AnyAsync(roomPlayers => roomPlayers.FirstPlayerId == userId 
        //                               || roomPlayers.SecondPlayerId == userId);
        //
        // if (doesRoomExist)
        // {
        //     return new CustomException(ExceptionTemplates.TwinkRoom);
        // }
        //     
        // var room = new Room
        // {
        //     IsPrivate = isPrivate,
        //     Code = Guid.NewGuid().ToString("n")[..8],
        //     IsFull = false,
        //     CreationTimeTicks = DateTimeOffset.Now.Ticks,
        //     Player = new Player
        //     {
        //         FirstPlayerId = userId,
        //         Count = 1
        //     }
        // }; 
        //
        // // await _rooms.AddAsync(room);
        // // await _repository.SaveChangesAsync();
        //     
        // // var newRoomPlayers = new RoomPlayers
        // // {
        // //     RoomId = room.Id,
        // //     FirstPlayerId = userId,
        // //     PlayersCount = 1
        // // };
        // //     
        // if (isTraining)
        // {
        //     room.Player.SecondPlayerId = 0;
        // }
        //
        // _repository.Add(room);
        //
        // // room.RoomPlayers = newRoomPlayers;
        // // _rooms.Update(room);
        // //     
        // await _repository.SaveChangesAsync();
        //     
        // return room.Adapt<RoomModel>();
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
        var room = await _rooms.FindAsync(roomId);

        return room is not null 
            ? room.Adapt<RoomModel>() 
            : new CustomException(ExceptionTemplates.RoomNotExists);
    }
        
    public async Task<int?> UpdateRoom(RoomModel room)
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

    /// <summary>
    /// Only to use with I HOSTED SERVICE!        
    /// </summary>
    /// <param name="roomOutDate"></param>
    /// <param name="roundOutDate"></param>
    /// <returns></returns>
    [Obsolete(message:"Should be carefully used")]
    public async Task<int> RemoveEntityRangeByDate(TimeSpan roomOutDate, TimeSpan roundOutDate)
    {
        return 0;
        throw new NotImplementedException();
        // var currentDate = DateTimeOffset.Now.Ticks;
        //    
        // var rooms = await _rooms
        //     .Where(room => room.CreationTimeTicks + roomOutDate.Ticks < currentDate && room.RoundId == null)
        //     .ToArrayAsync();
        //     
        // var allRound = await _repository.Rounds
        //     .Where(x => x.LastMoveTicks + roundOutDate.Ticks < currentDate)
        //     .ToArrayAsync();
        //     
        // _rooms.RemoveRange(rooms);
        // _repository.Rounds.RemoveRange(allRound);
        //     
        // await _repository.SaveChangesAsync();
        //
        // return rooms.Length + allRound.Length;
    }
}