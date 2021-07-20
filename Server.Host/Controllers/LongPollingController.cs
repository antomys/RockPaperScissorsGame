using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Bll.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/v1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LongPollingController : ControllerBase
    {
        private readonly ILongPollingService _longPollingService;

        public LongPollingController(ILongPollingService longPollingService)
        {
            _longPollingService = longPollingService;
        }

        [HttpGet("room")]
        public async Task<bool> CheckRoomState([FromQuery] int roomId)
        {
            return await _longPollingService.CheckRoomState(roomId);
        }
        
        [HttpGet("round")]
        public async Task<bool> CheckRoundState(int roundId)
        {
            return await _longPollingService.CheckRoundState(roundId);
        }
    }
}