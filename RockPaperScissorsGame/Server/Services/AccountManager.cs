using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Logging;
using RockPaperScissors.Models;
using Server.Models;
using Services;

namespace Server.Services
{
    public class AccountManager : IAccountManager
    {
        private readonly ILogger<AccountManager> _logger;

        private readonly ConcurrentDictionary<string, Account> _accountsActive = new();

        private readonly ConcurrentDictionary<string, int> _invalidTries = new();
        private readonly ConcurrentDictionary<string, DateTime> _lastTimes = new(); //todo: panic
        
        private readonly IDeserializedObject<Account> _deserializedObject;
        

        public AccountManager(
            ILogger<AccountManager> logger,
            IDeserializedObject<Account> deserializedObject)
        {
            _logger = logger;
            _deserializedObject = deserializedObject;
        }
        
        public async Task<Account> LogInAsync(AccountDto accountDto)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var invalidTryAccount = _invalidTries.FirstOrDefault(x => x.Key == accountDto.SessionId);
                if (invalidTryAccount.Value >= 2)
                {
                    if ((DateTime.Now - _lastTimes.FirstOrDefault(x=> x.Key == accountDto.SessionId).Value).TotalSeconds >= 45)
                    {
                        //todo: add logger
                        _invalidTries.TryRemove(invalidTryAccount);
                    }
                    else
                    {
                        _lastTimes.AddOrUpdate(accountDto.SessionId, accountDto.LastRequest, ((s, time) => time = accountDto.LastRequest));
                        return null;
                    }
                }
                var login = _deserializedObject.ConcurrentDictionary.Values
                    .FirstOrDefault(x => x.Login == accountDto.Login && x.Password == accountDto.Password);

                if (login == null || _accountsActive.ContainsKey(login.Id))
                {
                    _invalidTries.AddOrUpdate(accountDto.SessionId, 1, (s, i) => i+1);
                    _lastTimes.AddOrUpdate(accountDto.SessionId, accountDto.LastRequest, ((s, time) => time = accountDto.LastRequest));
                    return null;
                }
                
                _accountsActive.TryAdd(login.Id, login);
                _logger.LogTrace(""); //todo
               
                return login;
            });

            /*await Task.WhenAll(tasks);*/

            return await tasks;
        }
        
        
        /*public async Task<int> LogOutAsync()
        {
            
        }*/
    }
}