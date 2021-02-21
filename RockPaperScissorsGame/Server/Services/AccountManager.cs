using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Server.Models;
using Services;

namespace Server.Services
{
    public class AccountManager : IAccountManager
    {
        private readonly ILogger<AccountManager> _logger;

        private readonly ConcurrentDictionary<string, Account> _accountsActive = new();
        
        private readonly IDeserializedObject<Account> _deserializedObject; //todo: change into something else.

        public AccountManager(
            ILogger<AccountManager> logger,
            IDeserializedObject<Account> deserializedObject)
        {
            _logger = logger;
            _deserializedObject = deserializedObject;
        }
        
        public async Task<Account> LogInAsync(string name, string password)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var login = _deserializedObject.ConcurrentDictionary.Values
                    .FirstOrDefault(x => x.Login == name && x.Password == password);

                if (login == null || _accountsActive.ContainsKey(login.Id))
                {
                    _logger.LogTrace(""); //todo
                    
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