using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    [Route("/round")]
    [Consumes(MediaTypeNames.Application.Json)]
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
        [Route("get/{roundId}")]
        [ProducesResponseType(typeof(Round), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Round>> GetActualActiveRoundForCurrentRoom(string id)
        {
            try
            {
                var resultRoom = await _roundManager.GetCurrentActiveRoundForSpecialRoom(id);
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
        private readonly IRoundCoordinator _roundManager;
        private readonly ILogger<RoundController> _logger;
    }
}
