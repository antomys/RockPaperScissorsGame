using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Server.Contracts;
using Server.Exceptions.LogIn;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class AccountManager : IAccountManager
    {
        private readonly ILogger<AccountManager> _logger;


        private readonly ConcurrentDictionary<string, int> _invalidTries = new();
        private readonly ConcurrentDictionary<string, DateTime> _lastTimes = new(); //What am i doing? so stupid

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

        /// <summary>
        /// Method to asynchronously sign in
        /// </summary>
        /// <param name="accountDto">account from client</param>
        /// <returns>Account on server</returns>
        /// <exception cref="LoginCooldownException">When too many false retries</exception>
        /// <exception cref="InvalidCredentialsException">When invalid data</exception>
        /// <exception cref="UserAlreadySignedInException">When used is already signed in</exception>
        public Task<string> LogInAsync(AccountDto accountDto)
        { 
            var invalidTryAccount = _invalidTries.FirstOrDefault(x => x.Key == accountDto.Login);

            if (invalidTryAccount.Value >= 2)
            {
                if ((DateTime.Now - _lastTimes
                        .FirstOrDefault(x => x.Key == accountDto.Login).Value)
                    .TotalSeconds >= CoolDownTime)
                {
                    _invalidTries.TryRemove(invalidTryAccount);
                }
                else
                {
                    _lastTimes.AddOrUpdate(accountDto.Login, accountDto.LastRequest,
                        ((s, time) => time = accountDto.LastRequest));
                    throw new LoginCooldownException("CoolDown", CoolDownTime);
                }
            }

            var login = _deserializedObject.ConcurrentDictionary.Values
                .FirstOrDefault(x => x.Login == accountDto.Login && x.Password == accountDto.Password);

            if (login == null)
            {
                _invalidTries.AddOrUpdate(accountDto.Login, 1, (s, i) => i + 1);

                _lastTimes.AddOrUpdate(accountDto.Login, accountDto.LastRequest,
                    ((s, time) => time = accountDto.LastRequest));

                throw new InvalidCredentialsException($"{accountDto.Login}");
            }

            if (AccountsActive.Any(x => x.Value == login))
            {
                throw new UserAlreadySignedInException(nameof(login.Login));
            }

            var sessionId = Guid.NewGuid().ToString();
            AccountsActive.TryAdd(sessionId, login);
            _logger.LogTrace($"{sessionId} to {login}"); //todo

            return Task.FromResult(sessionId);
        }

        /// <summary>
        /// Async method to sign out of account
        /// </summary>
        /// <param name="sessionId">Session id of client</param>
        /// <returns>bool</returns>
        public async Task<bool> LogOutAsync(string sessionId)
        {
            var tasks = Task.Factory
                .StartNew(() => AccountsActive.ContainsKey(sessionId) && AccountsActive.TryRemove(sessionId, out _));

            return await tasks;
        }

        /// <summary>
        /// Checks if this session is active
        /// </summary>
        /// <param name="sessionId">Id of client session</param>
        /// <returns>bool</returns>
        public async Task<bool> IsActive(string sessionId)
        {
            var tasks = Task.Factory.StartNew(() => AccountsActive.ContainsKey(sessionId));
            return await tasks;
        }

        /// <summary>
        /// Gets active account from list of active accounts by id of client session
        /// </summary>
        /// <param name="sessionId">Id of client session</param>
        /// <returns>Account</returns>
        public Account GetActiveAccountBySessionId(string sessionId)
        {
            AccountsActive.TryGetValue(sessionId, out var account);

            return account;
        }
    }
}