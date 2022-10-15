using System.Text.Json.Serialization;

namespace RockPaperScissors.Common.Responses;

public sealed class LoginResponse
{
    /// <summary>
    ///     Gets or sets user token (used in header).
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; init; }
    
    /// <summary>
    ///     Gets or sets user login.
    /// </summary>
    [JsonPropertyName("login")]
    public string Login { get; init; }
}