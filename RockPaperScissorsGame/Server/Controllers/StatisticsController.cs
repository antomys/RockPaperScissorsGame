using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Mappings;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    
    [ApiController]
    [Route("/statistics")]
    public class OverallStatisticsController : ControllerBase
    {
        private readonly IDeserializedObject<Statistics> _deserializedStatistics;
        private readonly IAccountManager _accountManager;
        private readonly ILogger<OverallStatisticsController> _logger; //todo: add somewhere logger


        public OverallStatisticsController(
            IDeserializedObject<Statistics> deserializedStatistics,
            IAccountManager accountManager,
            ILogger<OverallStatisticsController> logger)
        {
            _deserializedStatistics = deserializedStatistics;
            _accountManager = accountManager;
            _logger = logger;
        }

        [HttpGet]
        [Route("overallStatistics")]
        [ProducesResponseType(typeof(IEnumerable<Statistics>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public  ActionResult<IEnumerable<Statistics>> GetOverallStatistics()
        {
            try
            {
                var deserialized = _deserializedStatistics.ConcurrentDictionary.Values;

                var statisticsList = deserialized.Where(x => x.Score > 10).AsParallel().ToArray();
                
                if (statisticsList.Length < 1)
                    return NotFound();
               
                var resultList = statisticsList.Select(statistics => statistics.ToStatisticsDto()).ToList();

                return Ok(resultList);
            }
            catch (Exception exceptions) //todo: custom exception OR NOT :)
            {
                return BadRequest(exceptions.Message);
            }
            
        }
        
        
        [HttpGet]
        [Route("personalStatistics/{sessionId}")]
        [ProducesResponseType(typeof(Statistics), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public  ActionResult<IEnumerable<Statistics>> GetPersonalStatistics(string sessionId)
        {
            try
            {
                var deserialized = _deserializedStatistics.ConcurrentDictionary.Values;

                var thisUserId = _accountManager.AccountsActive
                    .FirstOrDefault(x => x.Key.Equals(sessionId)).Value.Id;

                var resultStatistics = deserialized.FirstOrDefault(x => x.Id.Equals(thisUserId));
                
                return Ok(resultStatistics);
            }
            catch (Exception exceptions) //todo: custom exception
            {
                return BadRequest(exceptions.Message);
            }
            
        }
    }
}