namespace Server.Authentication.Models;

/// <summary>
///     Model for output of user account.
/// </summary>
public sealed class AccountOutputModel
{
    /// <summary>
    ///     Gets or sets user token (used in header).
    /// </summary>
    public string Token { get; set; }
    
    /// <summary>
    ///     Gets or sets user login.
    /// </summary>
    public string Login { get; set; }
}