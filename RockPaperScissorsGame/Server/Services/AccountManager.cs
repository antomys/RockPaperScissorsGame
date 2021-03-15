using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Server.Contracts;
using Server.Database;
using Server.Exceptions.LogIn;
using Server.Exceptions.Register;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class AccountManager : IAccountManager
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<AccountManager> _logger;
        
        private readonly ConcurrentDictionary<string, int> _invalidTries = new();
        private readonly ConcurrentDictionary<string, DateTime> _lastTimes = new(); //What am i doing? so stupid
        
        private const int CoolDownTime = 45;
        
        //Where string is sessionId and Account is his account credentials
        private ConcurrentDictionary<string, Account> AccountsActive { get; set; }

        public AccountManager(
            ILogger<AccountManager> logger,
            ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            AccountsActive = new ConcurrentDictionary<string, Account>();
        }

        public async Task<bool> RegisterAsync(string login, string password)
        {
            if(string.IsNullOrEmpty(login))
                throw new InvalidCredentialsException(nameof(login));
            if(string.IsNullOrEmpty(password))
                throw new InvalidCredentialsException(nameof(password));
            
            if(_applicationDbContext.Accounts.Any(x=> x.Login == login))
                throw new AlreadyExistsException(login);
            await _applicationDbContext.Accounts.AddAsync(new Account(login, password));

            await _applicationDbContext.SaveChangesAsync();

            return true;
        }

        Task<string> IAccountManager.LogInAsync(AccountDto accountDto)
        {
            throw new NotImplementedException();

        }

        public Task<Account> LogInAsync(AccountDto accountDto)
        {
            /*var login = _deserializedObject.ConcurrentDictionary.Values
                .FirstOrDefault(x => x.Login == accountDto.Login && x.Password == accountDto.Password);

            if (login == null)
            {
                _invalidTries.AddOrUpdate(accountDto.SessionId, 1, (s, i) => i + 1);

                _lastTimes.AddOrUpdate(accountDto.SessionId, accountDto.LastRequest,
                    ((s, time) => time = accountDto.LastRequest));

                throw new InvalidCredentialsException($"{accountDto.Login}");
            }

            if (AccountsActive.Any(x => x.Value == login)
                || AccountsActive.ContainsKey(accountDto.SessionId))

            {
                var thisAccount = AccountsActive.FirstOrDefault(x => x.Key == accountDto.SessionId).Value;
                AccountsActive.TryUpdate(accountDto.SessionId, login, thisAccount);
                //throw new UserAlreadySignedInException(nameof(login.Login));
            }

            AccountsActive.TryAdd(accountDto.SessionId, login);
            _logger.LogTrace(""); //todo

            return Task.FromResult(login);*/
            throw new NotImplementedException();
        }
        
        public async Task<bool> LogOutAsync(string sessionId)
        {
            //var tasks = Task.Factory
            //    .StartNew(() => AccountsActive.ContainsKey(sessionId) && AccountsActive.TryRemove(sessionId, out _));

            return await Task.FromResult(AccountsActive.ContainsKey(sessionId) && AccountsActive.TryRemove(sessionId, out _));

        }

        public async Task<bool> IsActiveAsync(string sessionId)
        {
            //var tasks = Task.Factory.StartNew(() => AccountsActive.ContainsKey(sessionId));
            
            return await Task.FromResult(AccountsActive.ContainsKey(sessionId));
        }
        
        public Account GetActiveAccountBySessionId(string sessionId)
        {
            AccountsActive.TryGetValue(sessionId, out var account);

            return account;
        }
    }
}