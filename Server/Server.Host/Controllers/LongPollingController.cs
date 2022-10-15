using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Bll.Services.Interfaces;

namespace Server.Host.Controllers;

[ApiController]
[Route ("api")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class LongPollingController : ControllerBase
{
    private readonly ILongPollingService _longPollingService;

    public LongPollingController(ILongPollingService longPollingService)
    {
        _longPollingService = longPollingService ?? throw new ArgumentNullException(nameof(longPollingService));
    }

    [HttpGet("Room/{roomId}/status")]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    public Task<long> CheckRoomUpdateTicksAsync(string roomId)
    {
        return _longPollingService.GetRoomUpdateTicksAsync(roomId);
    }
        
    [HttpGet("Round/{roundId}/status")]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    public Task<long> CheckRoundUpdateTicksAsync(string roundId)
    {
        return _longPollingService.GetRoundUpdateTicksAsync(roundId);
    }
}