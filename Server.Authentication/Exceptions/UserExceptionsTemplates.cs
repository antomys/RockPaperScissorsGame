using System;

namespace Server.Authentication.Exceptions;

internal static class UserExceptionsTemplates
{
    internal static string UserCoolDown(this string userName, DateTimeOffset date) => 
        $"User [{userName}] has been cooled down till [{date}].";
    
    internal static  string UserInvalidCredentials(this string userCredentials) =>
        $"Invalid Credentials for user [{userCredentials}].";
    
    internal static string UserNotFound(this string userName) => $"User with login [{userName}] is not found.";
    
    internal static string UserAlreadyExists(this string userName) => $"User with login [{userName}] already exists.";
    
    internal const string UnknownError = "Unknown error occured. Please try again.";    
}