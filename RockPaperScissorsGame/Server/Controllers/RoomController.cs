using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Game.Models;
using Server.Game.Services;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route ("/room")]
    
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class RoomController :ControllerBase
    {
        private readonly IRoomManager _roomManager;
        
        private readonly ILogger<RoomController> _logger;
        


        public RoomController(
            IRoomManager roomManager,
            ILogger<RoomController> logger)
        {
            _roomManager = roomManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("create/{sessionId}&{isPrivate}")]
        [ProducesResponseType(typeof(Room), (int) HttpStatusCode.OK)]
        [ProducesResponseType( (int) HttpStatusCode.BadRequest)]

        public async Task<ActionResult<Room>> CreateRoom(string sessionId, bool isPrivate)
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
        [Route("create/training/{sessionId}")]
        [ProducesResponseType(typeof(Room), (int) HttpStatusCode.OK)]
        [ProducesResponseType( (int) HttpStatusCode.BadRequest)]

        public async Task<ActionResult<Room>> CreateTrainingRoom(string sessionId)
        {
            try
            {
                var resultRoom = await _roomManager.CreateTrainingRoom(sessionId);

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
        [Route("join/private/{sessionId}&{roomId}")]
        [ProducesResponseType(typeof(Room), (int) HttpStatusCode.OK)]
        [ProducesResponseType( (int) HttpStatusCode.BadRequest)]

        public async Task<ActionResult<Room>> JoinRoom(string sessionId, string roomId)
        {
            try
            {
                var resultRoom = await _roomManager.JoinPrivateRoom( sessionId, roomId);

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
        [Route("updateState/{sessionId}&{state}")]
        [ProducesResponseType(typeof(Room), (int) HttpStatusCode.OK)]
        [ProducesResponseType( (int) HttpStatusCode.BadRequest)]

        public async Task<ActionResult<Room>> UpdatePlayerState(string sessionId, bool state)
        {
            try
            {
                var resultRound = await _roomManager.UpdatePlayerState(sessionId, state);

                if (resultRound != null)
                {
                    return resultRound;
                }

                return BadRequest();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
            
        }
        
        [HttpPut]
        [Route("update/{sessionId}")]
        [ProducesResponseType(typeof(Room), (int) HttpStatusCode.OK)]
        [ProducesResponseType( (int) HttpStatusCode.BadRequest)]

        public async Task<ActionResult<Room>> UpdateRoom(string sessionId)
        {
            try
            {
                var resultRoom = await _roomManager.UpdateRoom(sessionId);

                return resultRoom;
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
            
        }

        [HttpDelete]
        [Route("delete/{roomId}")]
        [ProducesResponseType(typeof(Room), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]

        public async Task<IActionResult> DeleteRoom(string roomId)
        {
           var result =  await _roomManager.DeleteRoom(roomId);
           if (result)
               return Ok();
           return BadRequest();
        }
    }
}