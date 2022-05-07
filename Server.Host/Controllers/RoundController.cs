﻿using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Models.Interfaces;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;

namespace Server.Host.Controllers;

/// <summary>
/// API Round Controller
/// </summary>
[ApiController]
[Route ("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class RoundController:ControllerBase
{
    private readonly IApplicationUser _applicationUser;
    private readonly IRoundService _roundService;
    private string UserId => _applicationUser.Id;
    public RoundController(
        IRoundService roundService, 
        IApplicationUser applicationUser)
    {
        _roundService = roundService;
        _applicationUser = applicationUser;
    }
    /// <summary>
    /// Creates round in room
    /// </summary>
    /// <param name="roomId">id of the room</param>
    /// <returns></returns>
    [HttpPost("create")]
    //[ProducesResponseType(typeof(Round), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateRound(int roomId)
    {
        throw new NotImplementedException();
        // var result = await _roundService.CreateAsync(UserId, roomId);
        // return result.Match<IActionResult>(
        //     Ok,
        //     exception => BadRequest(exception));
    }       
    /// <summary>
    /// Updates current room (Patches).
    /// </summary>
    /// <param name="roundModel">This round model from FE or client.</param>
    /// <returns></returns>
    [HttpPatch("update")]
    //[ProducesResponseType(typeof(Round), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateCurrentRound(RoundModel roundModel)
    {
        throw new NotImplementedException();
        // var updateResult = await _roundService.UpdateAsync(UserId, roundModel);
        // return updateResult.Match<IActionResult>(
        //     Ok,
        //     exception => BadRequest(exception));
    }
}