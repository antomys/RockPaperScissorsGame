using Microsoft.AspNetCore.Mvc;
using RockPaperScissors.Common;
using RockPaperScissors.Common.Enums;
using Server.Bll.Services.Interfaces;

namespace Server.Host.Controllers;

/// <summary>
/// API Round Controller
/// </summary>
public sealed class RoundController: ControllerBase
{
    private readonly IRoundService _roundService;

    public RoundController(IRoundService roundService)
    {
        _roundService = roundService ?? throw new ArgumentNullException(nameof(roundService));
    }

    [HttpGet(UrlTemplates.CheckRoundUpdateTicks)]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    public Task<long> CheckUpdateTicksAsync(string roundId)
    {
        return _roundService.GetUpdateTicksAsync(roundId);
    }
    
    [HttpGet(UrlTemplates.MakeMove)]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    public async Task<IActionResult> MakeMoveAsync(string roundId, Move move)
    {
        var makeMove = await _roundService.MakeMoveAsync(UserId, roundId, move);

        return makeMove.Match<IActionResult>(_ => Ok(), BadRequest);
    }
}