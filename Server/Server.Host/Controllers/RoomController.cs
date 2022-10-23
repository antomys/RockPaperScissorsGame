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

    [HttpPost(UrlTemplates.DeleteRoom)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync(string roomId)
    {
        var deleteResponse = await _roomService.DeleteAsync(UserId, roomId);
            
        return deleteResponse.Match<IActionResult>(
            _ => Ok(),
            BadRequest);
    }
    
    [HttpPost(UrlTemplates.ChangeStatus)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePlayerStatusAsync(string roomId, [FromQuery] bool newStatus)
    {
        var changePlayerStatus = await _roomService.ChangePlayerStatusAsync(UserId, roomId, newStatus);
            
        return changePlayerStatus.Match<IActionResult>(
            Ok,
            BadRequest);
    }

    [HttpGet(UrlTemplates.CheckRoomUpdateTicks)]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    public Task<long> CheckUpdateTicksAsync(string roomId)
    {
        return _roomService.GetUpdateTicksAsync(roomId);
    }
}