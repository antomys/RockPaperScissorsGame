using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Server.GameLogic.LogicServices;
using Server.GameLogic.Models.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("/room")]
    //[Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class RoomController : ControllerBase
    {
        private readonly IRoomCoordinator _roomManager;

        private readonly ILogger<RoomController> _logger;



        public RoomController(
            IRoomCoordinator roomManager,
            ILogger<RoomController> logger)
        {
            _roomManager = roomManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("create/{sessionId}&{isPrivate}")]
        [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
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
        [Route("join/{sessionId}&{roomId}")]
        [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Room>> JoinPrivateRoom(string sessionId, string roomId)
        {
            try
            {
                var resultRoom = await _roomManager.JoinPrivateRoom(sessionId, roomId);
                
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
        
        [HttpGet]
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
    }
}
