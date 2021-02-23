using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Contracts;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    
    [ApiController]
    [Route("/overallStatistics")]
    public class OverallStatisticsController : ControllerBase
    {
        private readonly IDeserializedObject<StatisticsDto> _deserializedStatistics;
        private readonly ILogger<OverallStatisticsController> _logger; //todo: add somewhere logger


        public OverallStatisticsController(
            IDeserializedObject<StatisticsDto> deserializedStatistics,
            ILogger<OverallStatisticsController> logger)
        {
            _deserializedStatistics = deserializedStatistics;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Statistics>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public  ActionResult<IEnumerable<Statistics>> GetOverallStatistics()
        {
            try
            {
                var deserialized = _deserializedStatistics.ConcurrentDictionary.Values;

                var resultList = deserialized.Where(x => x.Score > 10).AsParallel().ToArray();
                if (resultList.Length < 1)
                    return BadRequest("No statistics with points > 10");
                return Ok(resultList);
            }
            catch (Exception exceptions) //todo: custom exception
            {
                return BadRequest(exceptions.Message);
            }
            
        }
    }
}