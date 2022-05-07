using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Models.Interfaces;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;
using Server.Host.Contracts;

namespace Server.Host.Controllers;

[ApiController]
[Route ("api/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;
    private readonly IApplicationUser _applicationUser;
    private int UserId => _applicationUser.Id;
    public StatisticsController(IStatisticsService statisticsService, IApplicationUser applicationUser)
    {
        _statisticsService = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));
        _applicationUser = applicationUser ?? throw new ArgumentNullException(nameof(applicationUser));
    }

    [HttpGet("all")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<StatisticsDto>), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public Task<IEnumerable<ShortStatisticsModel>> GetOverallStatistics()
    {
        return _statisticsService.GetAllStatistics();
    }
        
    [HttpGet("personal")]
    [Authorize]
    //[ProducesResponseType(typeof(Statistics), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetPersonalStatistics()
    {
        return Ok(await _statisticsService.GetPersonalStatistics(UserId));
    }
}