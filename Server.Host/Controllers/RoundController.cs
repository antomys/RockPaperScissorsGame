﻿using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Authentication.Models.Interfaces;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;

namespace Server.Controllers
{
    /// <summary>
    /// API Round Controller
    /// </summary>
    [ApiController]
    [Route("api/v1/round")]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoundController:ControllerBase
    {
        private readonly ILogger<RoundController> _logger;
        private readonly IApplicationUser _applicationUser;
        private readonly IRoundService _roundService;
        private int UserId => _applicationUser.Id;
        public RoundController(
            ILogger<RoundController> logger, 
            IRoundService roundService, 
            IApplicationUser applicationUser)
        {
            _logger = logger;
            _roundService = roundService;
            _applicationUser = applicationUser;
        }
        /// <summary>
        /// Creates round in room
        /// </summary>
        /// <param name="roomId">id of the room</param>
        /// <returns></returns>
        [HttpGet("create")]
        //[ProducesResponseType(typeof(Round), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateRound(int roomId)
        {
            var result = await _roundService.CreateRoundAsync(UserId, roomId);
            return result.Match<IActionResult>(
                Ok,
                exception => BadRequest(exception));
        }       
        /// <summary>
        /// Updates current room (Patches).
        /// </summary>
        /// <param name="roundModel">This round model from FE or client.</param>
        /// <returns></returns>
        [HttpPatch("update")]
        //[ProducesResponseType(typeof(Round), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateCurrentRound(RoundModel roundModel)
        {
            var updateResult = await _roundService.UpdateRoundAsync(UserId, roundModel);
            return updateResult.Match<IActionResult>(
                Ok,
                exception => BadRequest(exception));
        }
        
        /*[HttpPatch]
        //[ProducesResponseType(typeof(Round), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> MakeMove()
        {
            throw new NotImplementedException();
        }*/
    }
}
