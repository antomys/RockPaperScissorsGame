using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using RockPaperScissors.Models;
using Server.Mappings;
using Server.Models;
using Server.Services;
using Services;

namespace Server.Controllers
{
    [ApiController]
    [Route ("/user")]
    
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class UserController : ControllerBase
    {
        private readonly IStorage<Account> _accountStorage;

        private readonly IAccountManager _accountManager;


        public UserController(
            IStorage<Account> users,
            IAccountManager accountManager
            )
        {
            _accountStorage = users;
            _accountManager = accountManager;
        }
        
        [HttpGet]
        [Route("login")]
        [ProducesResponseType(typeof(Account),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<AccountDto>> Login()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();
            if (string.IsNullOrEmpty(body))
                return BadRequest("Bad request");

            var requestedAccount = JsonConvert.DeserializeObject<AccountDto>(body);

            var result = _accountManager.LogInAsync(requestedAccount.Login, requestedAccount.Password).Result;
            
            if (result == null)
            {
                return BadRequest("No account like this or account is already used"); //todo: redo or leave like this
            }

            return result.ToUserDto();
        }
        
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(Account), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Account>> CreateAccount() //some shit
        {
            using StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();
            if (string.IsNullOrEmpty(body))
                return BadRequest("Bad request");

            var requestedAccount = JsonConvert.DeserializeObject<AccountDto>(body);
            var account = new Account
            {
                Id = Guid.NewGuid().ToString(),
                Login = requestedAccount.Login,
                Password = requestedAccount.Password
            };

            //var returned = _accountStorage.Add(account);
            var returned = await _accountStorage.AddAsync(account);
            if (returned != 400)
            {
                return account;
            }

            return BadRequest("This account already exists");
        }
    }
}