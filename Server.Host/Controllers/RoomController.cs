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
    [Route("[controller]/[action]")]
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

        [HttpPost]
        //[ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateRoom()
        {
            var newRoom = await _roomService.CreateRoom(UserId);

            return newRoom.Match<IActionResult>(
                Ok,
                exception => BadRequest(exception));
        }
        
        [HttpPost]
        //[ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> JoinRoom([FromQuery] string roomCode)
        {
            var result = await _roomService.JoinRoom(UserId, roomCode);
            return result.Match<IActionResult>(
                Ok,
                exception => BadRequest(exception));
        }
        
        [HttpGet]
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
        
        [HttpDelete]
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
