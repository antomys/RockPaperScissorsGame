using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Database;
using Server.Exceptions.LogIn;
using Server.Exceptions.Registration;
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
        public AccountManager(
            ILogger<AccountManager> logger,
            ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> RegisterAsync(string login, string password)
        {
            if (string.IsNullOrEmpty(login))
                throw new InvalidCredentialsException(nameof(login));
            if(string.IsNullOrEmpty(password))
                throw new InvalidCredentialsException(nameof(password));
            
            if(_applicationDbContext.Accounts.Any(x=> x.Login == login))
                throw new AlreadyExistsException(login);
            await _applicationDbContext.Accounts.AddAsync(new Account(login, password));

            await _applicationDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<string> LogInAsync(string login, string password)
        {
            if (string.IsNullOrEmpty(login))
                throw new InvalidCredentialsException(nameof(login));
            if(string.IsNullOrEmpty(password))
                throw new InvalidCredentialsException(nameof(password));
            
            var thisAccount = _applicationDbContext.Accounts.FirstOrDefault(x=> x.Login == login && x.Password == password);
            if (thisAccount == null) throw new InvalidCredentialsException(login +";"+ password);

            //Async????
            if (_applicationDbContext.ActiveSessionsEnumerable.Any(x=> x.AccountId == thisAccount.Id))
                throw new UserAlreadySignedInException(nameof(login));
            var sessionId = Guid.NewGuid().ToString();

            //Am i able to to something like that?

            await _applicationDbContext.ActiveSessionsEnumerable.AddAsync(new ActiveSessions(thisAccount.Id, sessionId));
            await _applicationDbContext.SaveChangesAsync();
            
            return sessionId;
        }
        
        public async Task<int> LogOutAsync(string sessionId)
        {
            try
            {
                if (!_applicationDbContext.ActiveSessionsEnumerable.Any(x => x.SessionId == sessionId))
                    return 0;
                
                var activeSession = new ActiveSessions { SessionId = sessionId };
                _applicationDbContext.ActiveSessionsEnumerable.Remove(activeSession);

                return await _applicationDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                _logger.LogError(exception.Message);
                return 0;
            }
        }
    }
}