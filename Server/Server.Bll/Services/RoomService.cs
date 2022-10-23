using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OneOf;
using RockPaperScissors.Common;
using Server.Bll.Exceptions;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;
using Server.Data.Context;
using Server.Data.Entities;
using Server.Data.Extensions;

namespace Server.Bll.Services;

internal sealed class RoomService : IRoomService
{
    private readonly ServerContext _repository;

    public RoomService(ServerContext repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<OneOf<RoomModel, CustomException>> CreateAsync(
        string userId, bool isPrivate, bool isTraining)
    {
        var doesRoomExist = await _repository.Rooms
            .Include(room => room.Players)
            .AnyAsync(roomPlayers => roomPlayers.Players.Any(player => player.AccountId == userId));

        if (doesRoomExist)
        {
            return new CustomException(ExceptionTemplates.TwinkRoom);
        }

        var players = new List<Player>(2)
        {
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Account = await _repository.Accounts.FindAsync(userId),
                AccountId = userId,
                IsReady = false,
            }
        };

        var room = new Room
        {
            Id = Guid.NewGuid().ToString(),
            IsPrivate = isPrivate,
            Code = Guid.NewGuid().ToString("N")[..8],
            IsFull = false,
            CreationTimeTicks = DateTimeOffset.UtcNow.Ticks,
            Players = players,
            UpdateTicks = DateTimeOffset.UtcNow.Ticks
        };

        _repository.Rooms.Add(room);

        if (isTraining)
        {
            room.IsFull = true;
            room.Players.Add(SeedingExtension.BotPlayer);
            room.Round = RoundService.Create(room);
        }

        await _repository.SaveChangesAsync();

        return room.Adapt<RoomModel>();
    }

    public async Task<OneOf<RoomModel, CustomException>> ChangeReadyStatusAsync(string userId, string roomId, bool newStatus)
    {
        var room = await _repository.Rooms
            .Include(room => room.Players)
            .FirstOrDefaultAsync(room => room.Id == roomId);

        if (room is null)
        {
            return new CustomException($"Room with id {roomId} does not exist");
        }

        var currentPlayer = room.Players.FirstOrDefault(player => player.Id == userId);
        if (currentPlayer is null)
        {
            return new CustomException($"You are not able to modify this room");
        }

        currentPlayer.IsReady = newStatus;

        _repository.Players.Update(currentPlayer);

        await _repository.SaveChangesAsync();

        if (room.Players.All(player => player.IsReady))
        {
            room.Round = RoundService.Create(room);
        }
        
        await _repository.SaveChangesAsync();
        
        return room.Adapt<RoomModel>();
    }
        
    public async Task<OneOf<RoomModel, CustomException>> JoinAsync(string userId, string? roomCode)
    {
        var oneOfRoom = string.IsNullOrEmpty(roomCode) ? await GetPublicAsync(userId) : await GetPrivateAsync(userId, roomCode);

        if (oneOfRoom.IsT1)
        {
            return oneOfRoom.AsT1;
        }

        var room = oneOfRoom.AsT0;

        var newPlayer = new Player
        {
            Id = Guid.NewGuid().ToString(),
            Account = await _repository.Accounts.FindAsync(userId),
            AccountId = userId,
            IsReady = false,
        };

        room.Players.Add(newPlayer);
        room.UpdateTicks = DateTimeOffset.UtcNow.Ticks;
        room.IsFull = room.Players.Count is 2;

        _repository.Rooms.Update(room);

        await _repository.SaveChangesAsync();

        return room.Adapt<RoomModel>();
    }

    public async Task<OneOf<RoomModel, CustomException>> GetAsync(string roomId)
    {
        var room = await _repository.Rooms.FindAsync(roomId);

        if (room is null)
        {
            return new CustomException($"Room with id {roomId} does not exist");
        }

        return room.Adapt<RoomModel>();
    }

    public async Task<OneOf<int, CustomException>> DeleteAsync(string userId, string roomId)
    {
        var room = await _repository.Rooms.FindAsync(roomId);

        if (room is null)
        {
            return new CustomException($"Room with id {roomId} does not exist");
        }

        if (room.Players.All(player => player.AccountId != userId))
        {
            return new CustomException("You have no rights to delete a room");
        }

        _repository.Rooms.Remove(room);

        await _repository.SaveChangesAsync();

        return StatusCodes.Status200OK;
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
    
    private async Task<OneOf<Room, CustomException>> GetPrivateAsync(string userId, string roomCode)
    {
        var room = await _repository.Rooms
            .Include(room => room.Players)
            .FirstOrDefaultAsync(room => room.Code == roomCode);

        if (room is null)
        {
            return new CustomException(ExceptionTemplates.NotExists(nameof(Room)));
        }
        
        if (room.IsFull)
        {
            return new CustomException(ExceptionTemplates.RoomFull);
        }
        
        if (room.Players.Any(player => player.AccountId == userId))
        {
            return new CustomException(ExceptionTemplates.AlreadyInRoom);
        }

        return room;
    }
    
    private async Task<OneOf<Room, CustomException>> GetPublicAsync(string userId)
    {
        var room = await _repository.Rooms
            .Include(room => room.Players)
            .FirstOrDefaultAsync(room => !room.IsFull && !room.IsPrivate && room.Players.All(player => player.AccountId != userId));

        if (room is null)
        {
            return new CustomException("There are no available free rooms");
        }
        
        return room;
    }
}