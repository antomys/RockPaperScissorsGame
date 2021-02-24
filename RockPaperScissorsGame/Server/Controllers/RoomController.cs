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
    [Consumes(MediaTypeNames.Application.Json)]
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
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<string>> CreateRoom(string sessionId, bool isPrivate)
        {
            try
            {
                var resultRoom = await _roomManager.CreateRoom(sessionId, isPrivate);
                if (resultRoom != null)
                {
                    return JsonConvert.SerializeObject(resultRoom);
                }
                return BadRequest();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }
        /*[HttpPut]
        [Route("update/{session}Id{&isReady}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<int>> UpdateRoom(string sessionId, bool isPrivate)
        {
            try
            {
                var resultRoom = await _roomManager.UpdateRoom(room);
                return resultRoom;
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }*/
    }
}
