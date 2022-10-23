using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Common;
using Server.Bll.Services.Interfaces;
using Server.Data.Context;
using Server.Data.Entities;
using Server.Data.Extensions;
using OneOf;
using RockPaperScissors.Common.Enums;

namespace Server.Bll.Services;

internal sealed class RoundService: IRoundService
{
    private readonly ServerContext _serverContext;

    public RoundService(ServerContext serverContext)
    {
        _serverContext = serverContext ?? throw new ArgumentNullException(nameof(serverContext));
    }

    public async Task<OneOf<bool, CustomException>> MakeMoveAsync(string userId, string roundId, Move move)
    {
        var round = await _serverContext.Rounds
            .Include(round => round.Players)
            .Include(round => round.Room)
            .FirstOrDefaultAsync(round => round.Id == roundId);

        if (round is null)
        {
            return new CustomException($"Unable to find round with id '{roundId}'");
        }

        var updateTicks = DateTimeOffset.UtcNow.Ticks;
        ProcessMoves(round, userId, move);

        round.UpdateTicks = updateTicks;
        round.Room.UpdateTicks = updateTicks;

        _serverContext.Update(round);

        await _serverContext.SaveChangesAsync();

        return true;
    }

    private void ProcessMoves(Round round, string userId, Move move)
    {
        var players = round.Players;
        var playingPlayer = players.First(player => player.Id == userId);
        var otherPlayer = players.First(player => player.Id != userId);

        if (otherPlayer.AccountId == SeedingExtension.BotId)
        {
            otherPlayer.Move = Random.Shared.Next(1, Enum.GetNames<Move>().Length);
        }

        playingPlayer.Move = (int)move;

        if (otherPlayer.Move is (int)Move.None)
        {
            return;
        }

        var playingPlayerMove = (Move)playingPlayer.Move;
        var otherPlayerMove = (Move)otherPlayer.Move;

        playingPlayer.PlayerState = playingPlayerMove switch
        {
            Move.Paper => otherPlayerMove switch
            {
                Move.Rock => Data.Entities.PlayerState.Win,
                Move.Scissors => Data.Entities.PlayerState.Lose,
                Move.Paper => Data.Entities.PlayerState.Draw,
                _ => Data.Entities.PlayerState.None,
            },
            Move.Rock => otherPlayerMove switch
            {
                Move.Rock => Data.Entities.PlayerState.Draw,
                Move.Scissors => Data.Entities.PlayerState.Win,
                Move.Paper => Data.Entities.PlayerState.Lose,
                _ => Data.Entities.PlayerState.None,
            },
            Move.Scissors => otherPlayerMove switch
            {
                Move.Rock => Data.Entities.PlayerState.Lose,
                Move.Scissors => Data.Entities.PlayerState.Draw,
                Move.Paper => Data.Entities.PlayerState.Win,
                _ => Data.Entities.PlayerState.None,
            },
            _ => Data.Entities.PlayerState.None,
        };

        otherPlayer.PlayerState = playingPlayer.PlayerState switch
        {
            Data.Entities.PlayerState.Win => Data.Entities.PlayerState.Lose,
            Data.Entities.PlayerState.Lose => Data.Entities.PlayerState.Win,
            Data.Entities.PlayerState.Draw => Data.Entities.PlayerState.Draw,
            _ => Data.Entities.PlayerState.None,
        };
    }

    public static Round Create(Room room)
    {
        var currentTime = DateTimeOffset.UtcNow.Ticks;
        var newRound = new Round
        {
            Id = Guid.NewGuid().ToString(),
            RoomId = room.Id,
            Room = room,
            Players = room.Players,
            IsFinished = false,
            StartTimeTicks = currentTime,
            FinishTimeTicks = 0,
            UpdateTicks = currentTime
        };

        return newRound;
    }
    
}