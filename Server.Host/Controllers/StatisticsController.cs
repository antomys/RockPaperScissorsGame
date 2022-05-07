using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Models.Interfaces;
using Server.Bll.Exceptions;
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
    
    private string UserId => _applicationUser.Id;
   
    public StatisticsController(IStatisticsService statisticsService, IApplicationUser applicationUser)
    {
        _statisticsService = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));
        _applicationUser = applicationUser ?? throw new ArgumentNullException(nameof(applicationUser));
    }

    [AllowAnonymous]
    [HttpGet("all")]
    [ProducesResponseType(typeof(ShortStatisticsModel[]), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public Task<ShortStatisticsModel[]> GetOverallStatistics()
    {
        return _statisticsService.GetAllStatistics();
    }
        
    [Authorize]
    [HttpGet("personal")]
    [ProducesResponseType(typeof(StatisticsModel), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CustomException), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetPersonalStatistics()
    {
        var result = await _statisticsService.GetPersonalStatistics(UserId);

        return result.Match<IActionResult>(
            statsModel => Ok(statsModel),
            statsException => BadRequest(statsException));
    }
}