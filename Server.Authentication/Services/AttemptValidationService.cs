using System;
using System.Collections.Concurrent;

namespace Server.Authentication.Services;

/// <summary>
///     Static service with validation features.
/// </summary>
internal static class AttemptValidationService
{
    // key - userId. Value - count of failed attempts
    private static readonly ConcurrentDictionary<string, int> FailedAttempts = new();
    private static readonly ConcurrentDictionary<string, DateTimeOffset> CoolDownCollection = new();

    public static bool TryInsertFailAttempt(this string userId)
    {
        if (FailedAttempts.TryGetValue(userId, out var failedAttempts))
        {
            if (failedAttempts >= 2)
            {
                // todo: in options
                CoolDownCollection.TryAdd(userId, DateTimeOffset.UtcNow.AddMinutes(2));
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
        if (!CoolDownCollection.TryGetValue(userId, out coolDownDate))
        {
            return false;
        }
        
        if (coolDownDate >= DateTimeOffset.UtcNow)
        {
            return true;
        }
        
        CoolDownCollection.TryRemove(userId, out coolDownDate);

        return false;
    }
}