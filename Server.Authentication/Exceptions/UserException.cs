using Microsoft.AspNetCore.Http;

namespace Server.Authentication.Exceptions;

/// <summary>
///     User-related custom structure exception.
/// </summary>
public sealed class UserException
{
    /// <summary>
    ///     Gets response code.
    /// </summary>
    public int Code { get; }
    
    /// <summary>
    ///     Gets exception message.
    /// </summary>
    public string Message { get; }
        
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public UserException(string message)
    {
        Code = StatusCodes.Status400BadRequest;
        Message = message;
    }
}