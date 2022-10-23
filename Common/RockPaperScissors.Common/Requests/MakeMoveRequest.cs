using RockPaperScissors.Common.Enums;

namespace RockPaperScissors.Common.Requests;

public sealed class MakeMoveRequest
{
    public string RoundId { get; init; }
    
    public Move Move { get; init; }
}