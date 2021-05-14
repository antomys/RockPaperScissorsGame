using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Contracts;

namespace Server.Controllers
{
    //todo: add accountDTO
    [ApiController]
    [Route ("[controller]/[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            ILogger<AccountController> logger)
        {
            _logger = logger;
        }       
        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<int>> Register(AccountDto accountDto)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<string>> Login(AccountDto accountDto)
        {
            throw new NotImplementedException();
        }
        
       
        
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.BadRequest)]
        public async Task<ActionResult<int>> Logout(string sessionId)
        {
            throw new NotImplementedException();
        }
    }
}