using RockPaperScissors.Common;
using OneOf;
using RockPaperScissors.Common.Enums;

namespace Server.Bll.Services.Interfaces;

public interface IRoundService
{
    Task<OneOf<bool, CustomException>> MakeMoveAsync(string userId, string roundId, Move move);
}