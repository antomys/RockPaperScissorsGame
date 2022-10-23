using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Server.Authentication;

/// <summary>
///     Options for JWT token..
/// </summary>
public sealed class AuthOptions
{
    private static readonly SymmetricSecurityKey DefaultKey = new(Encoding.ASCII.GetBytes(PrivateKey));
    
    /// <summary>
    ///     Token issuer (producer).
    /// </summary>
    public string Issuer { get; init; } = AppDomain.CurrentDomain.FriendlyName;

    /// <summary>
    ///     Token audience (consumer).
    /// </summary>
    public string Audience { get; init; } = "Player";

    /// <summary>
    ///     Token secret part.
    /// </summary>
    public static string PrivateKey => "RockPaperScissors";

    /// <summary>
    ///     Token life time.
    /// </summary>
    public TimeSpan LifeTime { get; init; }  = TimeSpan.FromHours(3);

    /// <summary>
    ///     Require HTTPS.
    /// </summary>
    public bool RequireHttps { get; init; } = false;

    /// <summary>
    ///     Getting a symmetric security key.
    /// </summary>
    /// <returns><see cref="SymmetricSecurityKey"/>.</returns>
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return DefaultKey;
    }
}