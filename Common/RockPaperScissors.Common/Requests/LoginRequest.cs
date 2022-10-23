using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RockPaperScissors.Common.Requests;

public sealed class LoginRequest
{
    [JsonPropertyName("login")]
    [Required(ErrorMessage = "Login is required!")]
    public string Login { get; init; }
    
    [JsonPropertyName("password")]
    [Required(ErrorMessage = "Password is required!!")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Invalid password length")]
    public string Password { get; init; }
    
    [JsonPropertyName("lastRequestTime")]
    public DateTimeOffset LastRequestTime { get; init; }
}