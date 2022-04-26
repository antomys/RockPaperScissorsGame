namespace Server.Authentication.Exceptions;

internal static class UserExceptionsTemplates
{
    internal const string UserCoolDown = "User [{0}] has been cooled down till [{1}].";
    internal const string UserInvalidCredentials = "Invalid Crenedtials for user [{0}].";
    internal const string UserNotFound = "User with login [{0}] is not found.";
    internal const string UserAlreadyExists = "User with login [{0}] already exists.";
    internal const string UnknownError = "Unknown error occured. Please try again.";    
}