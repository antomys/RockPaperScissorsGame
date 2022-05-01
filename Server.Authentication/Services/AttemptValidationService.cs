using System;
using System.Collections.Concurrent;

namespace Server.Authentication.Services;

public sealed class AttemptValidationService
{
    // key - userId. Value - failed attempts
    private readonly ConcurrentDictionary<string,int> _failedAttempts = new();
    private readonly ConcurrentDictionary<string, DateTimeOffset> _coolDownCollection = new();

    public bool InsertFailAttempt(string userId)
    {
        if (_failedAttempts.TryGetValue(userId, out var failedAttempts))
        {
            if (failedAttempts >= 2)
            {
                // todo: in options
                _coolDownCollection.TryAdd(userId, DateTimeOffset.Now.AddMinutes(1));
                _failedAttempts.TryRemove(userId, out _);
                
                return true;
            }
        }
        _failedAttempts.AddOrUpdate(userId, 1, (_, i) => i + 1);
            
        return true;
    }

    public int? CountFailedAttempts(string userId)
    {
        return _failedAttempts.TryGetValue(userId, out var failedAttempts)
            ? failedAttempts
            : default;
    }

    public bool IsCoolDown(string userId, out DateTimeOffset coolDownDate)
    {
        var result = _coolDownCollection.TryGetValue(userId, out coolDownDate);

        if (!result)
        {
            return false;
        }
        if (coolDownDate >= DateTimeOffset.Now)
        {
            return true;
        }
        _coolDownCollection.TryRemove(userId, out coolDownDate);

        return false;
    }
}