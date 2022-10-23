using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RockPaperScissors.Common;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;

namespace Server.Host.Controllers;

public sealed class RoomController: ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
    }

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
            Ok,
            BadRequest);
    }

    [HttpPost(UrlTemplates.JoinPublicRoom)]
    [ProducesResponseType(typeof(RoomModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> JoinPublicAsync()
    {
        var result = await _roomService.JoinAsync(UserId);
        
        return result.Match<IActionResult>(
            Ok,
            BadRequest);
    }

    [HttpPost(UrlTemplates.JoinPrivateRoom)]
    [ProducesResponseType(typeof(RoomModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> JoinPrivateAsync(string roomCode)
    {
        var result = await _roomService.JoinAsync(UserId, roomCode);
        
        return result.Match<IActionResult>(
            Ok,
            BadRequest);
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
            BadRequest);
    }
}