using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Exceptions;
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

    [HttpGet("room")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public Task<bool> CheckRoomState([FromQuery] string roomId)
    {
        return _longPollingService.CheckRoomState(roomId);
    }
        
    [HttpGet("round")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public Task<bool> CheckRoundState(string roundId)
    {
        return _longPollingService.CheckRoundState(roundId);
    }
}