using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoomController : ControllerBase
    {

        public RoomController()
        {
        }

        [HttpPost]
        //[ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateRoom()
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        //[ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> JoinRoom()
        {
            throw new NotImplementedException();
        }
        
        [HttpGet]
        //[ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateRoom()
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteRoom(string roomId)
        {
            throw new NotImplementedException();
        }
    }
}
