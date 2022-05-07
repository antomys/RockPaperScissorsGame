using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("room")]
    public Task<bool> CheckRoomState([FromQuery] int roomId)
    {
        return _longPollingService.CheckRoomState(roomId);
    }
        
    [HttpGet("round")]
    public Task<bool> CheckRoundState(int roundId)
    {
        return _longPollingService.CheckRoundState(roundId);
    }
}