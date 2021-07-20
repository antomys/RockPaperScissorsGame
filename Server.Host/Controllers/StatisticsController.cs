using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Models.Interfaces;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;
using Server.Contracts;

namespace Server.Controllers
{
    
    [ApiController]
    [Route("api/v1/stats")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        private readonly IApplicationUser _applicationUser;
        private int UserId => _applicationUser.Id;
        public StatisticsController(IStatisticsService statisticsService, IApplicationUser applicationUser)
        {
            _statisticsService = statisticsService;
            _applicationUser = applicationUser;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<StatisticsDto>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<StatisticsModel>> GetOverallStatistics()
        {
            return await _statisticsService.GetAllStatistics();
        }
        
        [HttpGet("personal")]
        [Authorize]
        //[ProducesResponseType(typeof(Statistics), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPersonalStatistics()
        {
            return Ok(await _statisticsService.GetPersonalStatistics(UserId));
        }
    }
}