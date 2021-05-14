using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;
using Server.Contracts;
using Server.Contracts.Requests;
using Server.Contracts.ViewModels;

namespace Server.Controllers
{
    [ApiController]
    [Route ("[controller]/[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(
            ILogger<AccountController> logger, 
            IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }       
        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            try
            {
                var result = _accountService.RegisterAsync(registerRequest.Adapt<AccountModel>());
                return Ok(await result);
            }
            catch (Exception exception)
            {
                return BadRequest();
            }
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