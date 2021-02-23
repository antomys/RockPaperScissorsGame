using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Game.Models;
using Server.Game.Services;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route ("/room")]
    
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class RoomController :ControllerBase
    {
        private readonly IRoomManager _roomManager;

        private readonly IStorage<Room> _roomStorage;

        private readonly ILogger<RoomController> _logger;

        private readonly IAccountManager _accountManager;


        public RoomController(
            IRoomManager roomManager,
            ILogger<RoomController> logger,
            IAccountManager accountManager,
            IStorage<Room> roomStorage)
        {
            _roomManager = roomManager;
            _logger = logger;
            _accountManager = accountManager;
            _roomStorage = roomStorage;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(Room), (int) HttpStatusCode.OK)]
        [ProducesResponseType( (int) HttpStatusCode.BadRequest)]

        public async Task<ActionResult<Room>> CreateRoom(string sessionId, bool isPrivate)
        {
            if (!_accountManager.AccountsActive.TryGetValue(sessionId, out var thisAccount))
            {
                return BadRequest();  //todo: return exception;
            }
            
            var resultRoom = await  _roomManager.CreateRoom(thisAccount, sessionId, isPrivate);

            if (resultRoom != null)
            {
                return resultRoom;
            }
            return BadRequest();
        }
        
        [HttpPost]
        [Route("join/private")]
        [ProducesResponseType(typeof(Room), (int) HttpStatusCode.OK)]
        [ProducesResponseType( (int) HttpStatusCode.BadRequest)]

        public async Task<ActionResult<Room>> JoinRoom(string sessionId, string roomId)
        {
            if (!_accountManager.AccountsActive.TryGetValue(sessionId, out var thisAccount))
            {
                return BadRequest();  //todo: return exception;
            }


            var resultRoom = await _roomManager.JoinPrivateRoom(thisAccount, sessionId, roomId);

            if (resultRoom != null)
            {
                return resultRoom;
            }
            return BadRequest();
        }
        
        [HttpPost]
        [Route("updateState")]
        [ProducesResponseType(typeof(Room), (int) HttpStatusCode.OK)]
        [ProducesResponseType( (int) HttpStatusCode.BadRequest)]

        public async Task<ActionResult<Room>> UpdatePlayerState(string sessionId, bool state)
        {
            if (!_accountManager.AccountsActive.TryGetValue(sessionId, out var thisAccount))
            {
                return BadRequest();  //todo: return exception;
            }


            var resultRoom = await _roomManager.UpdatePlayerState(thisAccount, state);

            if (resultRoom != null)
            {
                return resultRoom;
            }
            return BadRequest();
        }
        
        [HttpGet]
        [Route("update")]
        [ProducesResponseType(typeof(Room), (int) HttpStatusCode.OK)]
        [ProducesResponseType( (int) HttpStatusCode.BadRequest)]

        public async Task<ActionResult<Room>> UpdateRoom(string sessionId)
        {
            if (!_accountManager.AccountsActive.TryGetValue(sessionId, out var thisAccount))
            {
                return BadRequest();  //todo: return exception;
            }

            var resultRoom = await _roomManager.UpdateRoom(thisAccount.Login);

            return resultRoom;
        }
        
    }
}