using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class RoundController:ControllerBase
    {
        private readonly ILogger<RoundController> _logger;
        public RoundController(
            ILogger<RoundController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        //[ProducesResponseType(typeof(Round), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRound()
        {
            throw new NotImplementedException();

        }       
        [HttpGet]
        //[ProducesResponseType(typeof(Round), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateCurrentRound()
        {
            throw new NotImplementedException();
        }
        
        [HttpPatch]
        //[ProducesResponseType(typeof(Round), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> MakeMove()
        {
            throw new NotImplementedException();
        }
    }
}
