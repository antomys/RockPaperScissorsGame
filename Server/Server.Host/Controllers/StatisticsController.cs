using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RockPaperScissors.Common;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;

namespace Server.Host.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class StatisticsController: ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));
    }

    private string UserId => User.Identity?.Name ?? string.Empty;
    
    [AllowAnonymous]
    [HttpGet(UrlTemplates.AllStatistics)]
    [ProducesResponseType(typeof(ShortStatisticsModel[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<ShortStatisticsModel[]> GetOverallStatistics()
    {
        return _statisticsService.GetAllAsync();
    }
        
    [Authorize]
    [HttpGet(UrlTemplates.PersonalStatistics)]
    [ProducesResponseType(typeof(StatisticsModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomException), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPersonalStatistics()
    {
        var result = await _statisticsService.GetAsync(UserId);

        return result.Match<IActionResult>(
            statsModel => Ok(statsModel),
            statsException => BadRequest(statsException));
    }
}