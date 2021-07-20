using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Models.Interfaces;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/v1/room/")]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IApplicationUser _applicationUser;
        private int UserId => _applicationUser.Id;

        public RoomController(IRoomService roomService, 
            IApplicationUser applicationUser)
        {
            _roomService = roomService;
            _applicationUser = applicationUser;
        }

        [HttpPost("create")]
        //[ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateRoom([FromQuery] bool isPrivate)
        {
            var newRoom = await _roomService.CreateRoom(UserId, isPrivate);

            return newRoom.Match<IActionResult>(
                Ok,
                exception => BadRequest(exception));
        }

        [HttpPost("join/public")]
        public async Task<IActionResult> JoinPublicRoom()
        {
            var result = await _roomService.JoinRoom(UserId, true,null);
            return result.Match<IActionResult>(
                Ok,
                exception => BadRequest(exception));
        }
        [HttpPost("join/private")]
        //[ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> JoinPrivateRoom([FromQuery] string roomCode)
        {
            var result = await _roomService.JoinRoom(UserId, false, roomCode);
            return result.Match<IActionResult>(
                Ok,
                exception => BadRequest(exception));
        }
        
        [HttpGet("update")]
        //[ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateRoom([FromBody] RoomModel roomModel)
        {
            var updateResponse = await _roomService.UpdateRoom(roomModel);

            return updateResponse switch
            {
                200 => Ok(),
                _ => BadRequest()
            };
        }
        
        [HttpDelete("delete")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteRoom([FromQuery] int roomId)
        {
            var deleteResponse = await _roomService.DeleteRoom(UserId,roomId);
            
            return deleteResponse switch
            {
                200 => Ok(),
                _ => BadRequest()
            };
        }
    }
}
