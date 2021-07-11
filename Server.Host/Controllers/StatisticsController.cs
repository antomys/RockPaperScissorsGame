using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Contracts;

namespace Server.Controllers
{
    
    [ApiController]
    [Route("[controller]/[action]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ILogger<StatisticsController> _logger;


        public StatisticsController(
            ILogger<StatisticsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("overallStatistics")]
        [ProducesResponseType(typeof(IEnumerable<StatisticsDto>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public  ActionResult<IEnumerable<StatisticsDto>> GetOverallStatistics()
        {
            throw new NotImplementedException();
        }
        
        [HttpGet]
        //[ProducesResponseType(typeof(Statistics), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public  ActionResult<IActionResult> GetPersonalStatistics()
        {
            throw new NotImplementedException();
        }
    }
}