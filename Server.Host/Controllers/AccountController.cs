using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Services;
using Server.Contracts;
using Server.Contracts.Requests;

namespace Server.Controllers
{
    [ApiController]
    [Route ("[controller]/[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }       
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid) return BadRequest();
            var newAccount = await _authService
                .RegisterAsync(registerRequest.Login,registerRequest.Password);
            return newAccount.Match<IActionResult>(
                integer => Ok(integer),
                exception => BadRequest(exception));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(AccountDto accountDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var newAccount =
                await _authService.LoginAsync(accountDto.Login, accountDto.Password);
            
            return newAccount.Match<IActionResult>(
                Ok,
                userException => BadRequest(userException)
            );
        }
        
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.BadRequest)]
        public ActionResult<int> Logout(string sessionId)
        {
            return Ok(sessionId);
        }
    }
}