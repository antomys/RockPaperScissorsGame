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
public sealed class RoomController: ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
    }

    private string UserId => User.Identity?.Name ?? string.Empty;
    
    [HttpPost(UrlTemplates.CreateRoom)]
    [ProducesResponseType(typeof(RoomModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(
        [FromQuery] bool isPrivate, 
        [FromQuery] bool isTraining = false)
    {
        var newRoom = await _roomService
            .CreateAsync(UserId, isPrivate, isTraining);
        
        return newRoom.Match<IActionResult>(
            roomModel => Ok(roomModel),
            exception => BadRequest(exception));
    }

    [HttpPost(UrlTemplates.JoinPublicRoom)]
    [ProducesResponseType(typeof(RoomModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> JoinPublicAsync()
    {
        var result = await _roomService.JoinAsync(UserId);
        
        return result.Match<IActionResult>(
            roomModel => Ok(roomModel),
            exception => BadRequest(exception));
    }
    
    [HttpPost(UrlTemplates.JoinPrivateRoom)]
    [ProducesResponseType(typeof(RoomModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> JoinPrivateAsync(string roomCode)
    {
        var result = await _roomService.JoinAsync(UserId, roomCode);
        
        return result.Match<IActionResult>(
            roomModel => Ok(roomModel),
            exception => BadRequest(exception));
    }
        
    [HttpPost(UrlTemplates.UpdateRoom)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromBody] RoomModel roomModel)
    {
        var updateResponse = await _roomService.UpdateAsync(roomModel);
        
        return updateResponse switch
        {
            StatusCodes.Status200OK => Ok(),
            _ => BadRequest()
        };
    }
        
    [HttpDelete(UrlTemplates.DeleteRoom)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromQuery] string roomId)
    {
        var deleteResponse = await _roomService.DeleteAsync(UserId, roomId);
            
        return deleteResponse.Match<IActionResult>(
            _ => Ok(),
            exception => BadRequest(exception));
    }
}