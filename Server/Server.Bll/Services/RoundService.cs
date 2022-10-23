using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Common;
using Server.Bll.Services.Interfaces;
using Server.Data.Context;
using Server.Data.Entities;
using Server.Data.Extensions;
using OneOf;
using RockPaperScissors.Common.Enums;
using PlayerState = Server.Data.Entities.PlayerState;

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

        if (round.IsFinished)
        {
            return new CustomException($"Round has been finished.");
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
        var playingPlayer = players.FirstOrDefault(player => player.AccountId == userId);

        if (playingPlayer is null)
        {
            return;
        }
        
        var otherPlayer = players.First(player => player.AccountId != userId);

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
                Move.Rock => PlayerState.Win,
                Move.Scissors => PlayerState.Lose,
                Move.Paper => PlayerState.Draw,
                _ => PlayerState.None,
            },
            Move.Rock => otherPlayerMove switch
            {
                Move.Rock => PlayerState.Draw,
                Move.Scissors => PlayerState.Win,
                Move.Paper => PlayerState.Lose,
                _ => PlayerState.None,
            },
            Move.Scissors => otherPlayerMove switch
            {
                Move.Rock => PlayerState.Lose,
                Move.Scissors => PlayerState.Draw,
                Move.Paper => PlayerState.Win,
                _ => PlayerState.None,
            },
            _ => PlayerState.None,
        };

        otherPlayer.PlayerState = playingPlayer.PlayerState switch
        {
            PlayerState.Win => PlayerState.Lose,
            PlayerState.Lose => PlayerState.Win,
            PlayerState.Draw => PlayerState.Draw,
            _ => PlayerState.None,
        };

        if (playingPlayer.PlayerState is not PlayerState.None && otherPlayer.PlayerState is not PlayerState.None)
        {
            round.IsFinished = true;
        }
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