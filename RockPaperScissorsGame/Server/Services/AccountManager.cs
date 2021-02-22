using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RockPaperScissors.Models;
using Server.Exceptions.LogIn;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class AccountManager : IAccountManager
    {
        private readonly ILogger<AccountManager> _logger;
        

        private readonly ConcurrentDictionary<string, int> _invalidTries = new();
        private readonly ConcurrentDictionary<string, DateTime> _lastTimes = new(); //todo: panic
        
        private readonly IDeserializedObject<Account> _deserializedObject;
        private const int CoolDownTime = 45;
        
        public ConcurrentDictionary<string, Account> AccountsActive { get; set; }
        
        
        public AccountManager(
            ILogger<AccountManager> logger,
            IDeserializedObject<Account> deserializedObject)
        {
            _logger = logger;
            _deserializedObject = deserializedObject;
            AccountsActive = new ConcurrentDictionary<string, Account>();
        }


        

        public async Task<Account> LogInAsync(AccountDto accountDto)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var invalidTryAccount = _invalidTries.FirstOrDefault(x => x.Key == accountDto.SessionId);
                if (invalidTryAccount.Value >= 2)
                {
                    if ((DateTime.Now - _lastTimes.FirstOrDefault(x=> x.Key == accountDto.SessionId).Value).TotalSeconds >= CoolDownTime)
                    {
                        //todo: add logger
                        _invalidTries.TryRemove(invalidTryAccount);
                    }
                    else
                    {
                        // ReSharper disable once RedundantAssignment
                        _lastTimes.AddOrUpdate(accountDto.SessionId, accountDto.LastRequest, ((s, time) => time = accountDto.LastRequest));
                        throw new LoginCooldownException("CoolDown", CoolDownTime);
                    }
                }
                var login = _deserializedObject.ConcurrentDictionary.Values
                    .FirstOrDefault(x => x.Login == accountDto.Login && x.Password == accountDto.Password);

                if (login == null)
                {
                    _invalidTries.AddOrUpdate(accountDto.SessionId, 1, (s, i) => i+1);
                    // ReSharper disable once RedundantAssignment
                    _lastTimes.AddOrUpdate(accountDto.SessionId, accountDto.LastRequest, ((s, time) => time = accountDto.LastRequest));
                    throw new InvalidCredentialsException($"{accountDto.Login}");
                }
                if (AccountsActive.Any(x => x.Value == login)
                || AccountsActive.ContainsKey(accountDto.SessionId))
                {
                    throw new UserAlreadySignedInException(nameof(login.Login));
                }
                
                AccountsActive.TryAdd(accountDto.SessionId, login);
                _logger.LogTrace(""); //todo
               
                return login;
            });

            return await tasks;
        }

        public async Task<bool> LogOutAsync(string sessionId)
        {
            var tasks = Task.Factory
                .StartNew(() => AccountsActive.ContainsKey(sessionId) && AccountsActive.TryRemove(sessionId, out _));

            return await tasks;

        }



        /*public async Task<int> LogOutAsync()
        {
            
        }*/
    }
}