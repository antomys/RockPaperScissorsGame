namespace Server.Authentication.Exceptions;

/// <summary>
///     Compile-time exception templates for Authentication module.
/// </summary>
internal static class UserExceptionsTemplates
{
    /// <summary>
    ///     Builds exception template for Cooldown of Client login attempts.
    /// </summary>
    /// <param name="username">Client username.</param>
    /// <param name="tillCooldownDate">Date of removing cooldown.</param>
    /// <returns>Created string message.</returns>
    internal static string UserCoolDown(this string username, DateTimeOffset tillCooldownDate)
        => $"User [{username}] has been cooled down till \"{tillCooldownDate}\".";
    
    /// <summary>
    ///     Builds exception template for invalid credentials for user login attempt.
    /// </summary>
    /// <param name="username">Client input username.</param>
    /// <returns>Created string message.</returns>
    internal static  string UserInvalidCredentials(this string username)
        => $"Invalid Credentials for user \"{username}\".";
    
    /// <summary>
    ///     Builds exception template for not found client by input parameters.
    /// </summary>
    /// <param name="username">Client input username.</param>
    /// <returns>Created string message.</returns>
    internal static string UserNotFound(this string username)
        => $"User with login \"{username}\" is not found.";
    
    /// <summary>
    ///     Builds exception template for registering client, that already exists.
    /// </summary>
    /// <param name="username">Client input username.</param>
    /// <returns>Created string message.</returns>
    internal static string UserAlreadyExists(this string username)
        => $"User with login \"{username}\" already exists.";
    
    /// <summary>
    ///     Default template, when error is unknown.
    /// </summary>
    internal const string UnknownError = "Unknown error occured. Please try again.";    
}