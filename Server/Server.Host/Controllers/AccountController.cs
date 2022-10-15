using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RockPaperScissors.Common.Models;
using RockPaperScissors.Common.Requests;
using Server.Authentication.Exceptions;
using Server.Authentication.Models;
using Server.Authentication.Services;

namespace Server.Host.Controllers;

[ApiController]
[Route ("api/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public sealed class AccountController: ControllerBase
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }
    
    [HttpPost("register")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserException),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest)
    {
        var newAccount = await _authService
            .RegisterAsync(registerRequest.Login,registerRequest.Password);
        
        return newAccount.Match<IActionResult>(
            statusCode => Ok(statusCode.ToString()),
            userException => BadRequest(userException));
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AccountOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserException),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginAsync(AccountDto accountDto)
    {
        var newAccount =
            await _authService.LoginAsync(accountDto.Login, accountDto.Password);
            
        return newAccount.Match<IActionResult>(
            accountOutputModel => Ok(accountOutputModel),
            userException => BadRequest(userException));
    }
        
    [Authorize]
    [HttpGet("logout")]
    [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(int), (int) HttpStatusCode.BadRequest)]
    public ActionResult<string> Logout(string sessionId)
    {
        return sessionId;
    }
}