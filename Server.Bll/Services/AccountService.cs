using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Server.Exceptions.LogIn;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class AccountService : IAccountService
    {
        
        private readonly ConcurrentDictionary<string, int> _invalidTries = new();
        private readonly ConcurrentDictionary<string, DateTime> _lastTimes = new(); //What am i doing? so stupid

        private readonly IDeserializedObject<Account> _deserializedObject;
        private const int CoolDownTime = 45;
        public ConcurrentDictionary<string, Account> AccountsActive { get; set; }


        public AccountService()
        {

        }
        
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
        public async Task<bool> LogOutAsync(string sessionId)
        {
            var tasks = Task.Factory
                .StartNew(() => AccountsActive.ContainsKey(sessionId) && AccountsActive.TryRemove(sessionId, out _));

            return await tasks;
        }
        public async Task<bool> IsActive(string sessionId)
        {
            throw new NotImplementedException();
        }

        private Task RemoveOutdatedSession()
        {
            throw new NotImplementedException();
        }
    }
}