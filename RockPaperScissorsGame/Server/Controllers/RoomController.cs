using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Server.Exceptions.Room;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("/room")]
    [Produces(MediaTypeNames.Application.Json)]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomManager _roomManager;

        public RoomController(ILogger<RoomController> logger, IRoomManager roomManager)
        {
            _logger = logger;
            _roomManager = roomManager;
        }

        [HttpPost]
        [Route("create/")]
        [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Room>> CreateRoom(
            [FromQuery]string sessionId, 
            [FromQuery]bool isPrivate)
        {
            try
            {
                var resultRoom = await _roomManager.CreateRoom(sessionId, isPrivate);
                if (resultRoom != null)
                {
                    return resultRoom;
                }
                return BadRequest();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }
        
        [HttpPost]
        [Route("join/")]
        [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Room>> JoinRoom(
            [FromQuery]string sessionId, 
            [FromQuery]string roomId,
            [FromHeader]string roomType)
        {
            try
            {
                var resultRoom = await _roomManager.JoinRoom(sessionId,roomType, roomId);
                
                if (resultRoom != null)
                {
                    return resultRoom;
                }
                return BadRequest();
            }
            catch (RoomException exception)
            {
                _logger.LogError(exception,exception.Message);
                return BadRequest(exception.Message);
            }
        }
        
        /*[HttpGet]
        [Route("join/{sessionId}")]
        [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Room>> JoinPublicRoom(string sessionId)
        {
            try
            {
                var resultRoom = await _roomManager.JoinPublicRoom(sessionId);
                
                if (resultRoom != null)
                {
                    return resultRoom;
                }
                return BadRequest();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }
        
        [HttpPut]
        [Route("updateState/{sessionId}&{state}")]
        [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Room>> UpdatePlayerState(string sessionId, bool state)
        {
            try
            {
                var resultRound = await _roomManager.UpdatePlayerStatus(sessionId, state);
                if (resultRound != null)
                    return resultRound;
                return BadRequest();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
       
        [HttpGet]
        [Route("updateState/{roomId}")]
        [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Room>> Update(string roomId)
        {
            try
            {
                var resultRound = await _roomManager.UpdateRoom(roomId);
                return resultRound;
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
        
        [HttpGet]
        [Route("create/training/{sessionId}")]
        [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Room>> CreateTrainingRoom(string sessionId)
        {
            try
            {
                var resultRound = await _roomManager.CreateTrainingRoom(sessionId);
                return resultRound;
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
        [HttpDelete]
        [Route("delete/{roomId}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)] //Probably set new HttpStatus 
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<string>> DeleteRoomExit(string roomId)
        {
            try
            {
                var deleted = await _roomManager.DeleteRoom(roomId);
                if (deleted)
                    return Ok("Room was deleted successfully!");
                return BadRequest();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }*/
    }
}
