using System;
using System.Collections.Concurrent;

namespace Server.Authentication.Services;

internal static class AttemptValidationService
{
    // key - userId. Value - failed attempts
    private static readonly ConcurrentDictionary<string, int> FailedAttempts = new();
    private static readonly ConcurrentDictionary<string, DateTimeOffset> CoolDownCollection = new();

    public static bool TryInsertFailAttempt(this string userId)
    {
        if (FailedAttempts.TryGetValue(userId, out var failedAttempts))
        {
            if (failedAttempts >= 2)
            {
                // todo: in options
                CoolDownCollection.TryAdd(userId, DateTimeOffset.Now.AddMinutes(2));
                FailedAttempts.TryRemove(userId, out _);
                
                return true;
            }
        }
        FailedAttempts.AddOrUpdate(userId, 1, (_, i) => i + 1);
            
        return true;
    }

    public static int? CountFailedAttempts(this string userId)
    {
        return FailedAttempts.TryGetValue(userId, out var failedAttempts)
            ? failedAttempts
            : default;
    }

    public static bool IsCoolDown(this string userId, out DateTimeOffset coolDownDate)
    {
        var result = CoolDownCollection.TryGetValue(userId, out coolDownDate);

        if (!result)
        {
            return false;
        }
        if (coolDownDate >= DateTimeOffset.Now)
        {
            return true;
        }
        CoolDownCollection.TryRemove(userId, out coolDownDate);

        return false;
    }
}