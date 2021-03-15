﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Writers;
using Server.Contracts;
using Server.Exceptions.LogIn;
using Server.Exceptions.Registration;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route ("/user")]
    
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class UserController : ControllerBase
    {
        //private readonly IStorage<Account> _accountStorage;
        
        //private readonly IStorage<Statistics> _statisticsStorage; //Just to write new statistics field into file along with account

        private readonly ILogger<UserController> _logger;

        private readonly IAccountManager _accountManager;
        public UserController(
            IAccountManager accountManager, 
            ILogger<UserController> logger)
        {
            _accountManager = accountManager;
            _logger = logger;
        }       

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody]AccountDto accountDto)
        {
            try
            {
                var sessionId = await _accountManager.LogInAsync(accountDto.Login,accountDto.Password);
                return Ok(sessionId);
            }
            catch (ValidationException exception)
            {
                _logger.Log(LogLevel.Warning,"Validation error");
                return BadRequest(exception.Message);
            }
            catch (LoginErrorException exception)
            {
                _logger.Log(LogLevel.Error,exception.Message);
                return BadRequest(exception.Message);
            }
            
        }
        
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<int>> Register([FromBody]AccountDto accountDto)
        {
            try
            {
                await _accountManager.RegisterAsync(accountDto.Login, accountDto.Password);
                return Created("", $"Account [{accountDto.Login}] successfully created");
            }
            catch (ValidationException exception)
            {
                _logger.Log(LogLevel.Warning,exception.Message);
                return BadRequest(exception.Message);
            }
            catch (UnknownReasonException exception)
            {
                _logger.Log(LogLevel.Warning, exception.Message);
                return BadRequest(exception.Message);
            }
        }
        
        [Route("logout")]
        [HttpDelete("logout/{sessionId}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LogOut(string sessionId)
        {
            var result = await _accountManager.LogOutAsync(sessionId);
            if (result == 1)
                return Ok();
            return BadRequest();
        }
    }
}