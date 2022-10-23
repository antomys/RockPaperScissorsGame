using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RockPaperScissors.Common;
using RockPaperScissors.Common.Models;
using RockPaperScissors.Common.Requests;
using Server.Authentication.Exceptions;
using Server.Authentication.Models;
using Server.Authentication.Services;

namespace Server.Host.Controllers;

public sealed class AccountController: ControllerBase
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [AllowAnonymous]
    [HttpPost(UrlTemplates.RegisterUrl)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserException),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest)
    {
        var newAccount = await _authService
            .RegisterAsync(registerRequest.Login,registerRequest.Password);
        
        return newAccount.Match<IActionResult>(
            statusCode => Ok(statusCode.ToString()),
            BadRequest);
    }

    [AllowAnonymous]
    [HttpPost(UrlTemplates.LoginUrl)]
    [ProducesResponseType(typeof(AccountOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserException),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginAsync(AccountDto accountDto)
    {
        var newAccount =
            await _authService.LoginAsync(accountDto.Login, accountDto.Password);
            
        return newAccount.Match<IActionResult>(
            Ok,
            BadRequest);
    }

    [HttpGet(UrlTemplates.LogoutUrl)]
    [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(int), (int) HttpStatusCode.BadRequest)]
    public ActionResult<string> Logout(string sessionId)
    {
        return sessionId;
    }
}