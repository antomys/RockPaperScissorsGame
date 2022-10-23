using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RockPaperScissors.Common.Requests;

public sealed class RegisterRequest
{
    [JsonPropertyName("login")]
    [Required(ErrorMessage = "Login is required!")]
    public string Login { get; init; }
    
    [JsonPropertyName("password")]
    [Required(ErrorMessage = "Password is required!")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Invalid password length. Must be 6-20")]
    public string Password { get; init; }
}