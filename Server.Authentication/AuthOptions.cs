using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Server.Authentication;

/// <summary>
///     Options for JWT token..
/// </summary>
public sealed class AuthOptions
{
    /// <summary>
    ///     Token issuer (producer).
    /// </summary>
    public string Issuer { get; set; } = "Rock Paper Scissors";
        
    /// <summary>
    ///     Token audience (consumer).
    /// </summary>
    public string Audience { get; set; } = "Player";

    /// <summary>
    ///     Token secret part.
    /// </summary>
    public string PrivateKey { get; set; } = "RockPaperScissors";
        
    /// <summary>
    ///     Token life time.
    /// </summary>
    public TimeSpan LifeTime { get; set; }  = TimeSpan.FromHours(3);

    /// <summary>
    ///     Require HTTPS.
    /// </summary>
    public bool RequireHttps { get; set; } = false;

    /// <summary>
    ///     Getting a symmetric security key.
    /// </summary>
    /// <returns><see cref="SymmetricSecurityKey"/>.</returns>
    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(PrivateKey));
    }
}