using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Authentication.Services
{
    public class AttemptValidationService
    {
        // key - userId. Value - failed attempts
        private readonly ConcurrentDictionary<string,int> _failedAttempts = new();
        private readonly ConcurrentDictionary<string, DateTimeOffset> _coolDownCollection = new();

        public Task InsertFailAttempt(string userId)
        {
            if (_failedAttempts.TryGetValue(userId, out var failedAttempts))
            {
                if (failedAttempts >= 2)
                {
                    // todo: in options
                    _coolDownCollection.TryAdd(userId, DateTimeOffset.Now.AddMinutes(1));
                    _failedAttempts.TryRemove(userId, out _);
                    return Task.CompletedTask;
                }
            }
            _failedAttempts.AddOrUpdate(userId, 1, (s, i) => i + 1);
            
            return Task.CompletedTask;
        }

        public Task<int?> CountFailedAttempts(string userId)
        {
            _failedAttempts.TryGetValue(userId, out var failedAttempts);
            return Task.FromResult<int?>(failedAttempts);
        }

        public Task<bool> IsCoolDown(string userId, out DateTimeOffset coolDownDate)
        {
            var result = _coolDownCollection.TryGetValue(userId, out coolDownDate);
            if (!result) return Task.FromResult(false);
            if (coolDownDate >= DateTimeOffset.Now) return Task.FromResult(true);
            _coolDownCollection.TryRemove(userId, out coolDownDate);
            return Task.FromResult(false);
        }
    }
}