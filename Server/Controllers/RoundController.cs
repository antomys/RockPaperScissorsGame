using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.GameLogic.LogicServices;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Server.GameLogic.LogicServices.Interfaces;
using Server.GameLogic.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("/round")]
    [Produces(MediaTypeNames.Application.Json)]
    public class RoundController:ControllerBase
    {
        public RoundController(
               IRoundCoordinator roundManager,
               ILogger<RoundController> logger)
        {
            _roundManager = roundManager;
            _logger = logger;
        }
        [HttpGet]
        [Route("get/{roomId}")]
        [ProducesResponseType(typeof(Round), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Round>> GetActualActiveRoundForCurrentRoom(string roomId)
        {
            try
            {
                var resultedRound = await _roundManager.GetCurrentActiveRoundForSpecialRoom(roomId);
                if (resultedRound != null)
                {
                    return resultedRound;
                }
                return BadRequest();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }       
        [HttpGet]
        [Route("get/update/{roomId}")]
        [ProducesResponseType(typeof(Round), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Round>> UpdateCurrentRound(string roomId)
        {
            try
            {
                var resultedRound = await _roundManager.UpdateRound(roomId);
                
                if (resultedRound != null)
                {
                    return resultedRound;
                }
                return BadRequest();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }
        
        [HttpPatch]
        [Route("move/{roomId}&{sessionId}&{move}")]
        [ProducesResponseType(typeof(Round), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Round>> PlaceYourMoveToActiveRound(string roomId, string sessionId, int move)
        {
            try
            {
                var resultedRound = await _roundManager.MakeMove(roomId,sessionId,move);
                if (resultedRound != null)
                {
                    return resultedRound;
                }
                return BadRequest();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }

        private readonly IRoundCoordinator _roundManager;
        private readonly ILogger<RoundController> _logger;
    }
}
