namespace Server.Authentication.Models.Interfaces;

/// <summary>
///     Interface for ApplicationUser class
/// </summary>
public interface IApplicationUser
{
    /// <summary>
    ///     Gets user request id from token
    /// </summary>
    string Id { get; }
}