using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Host.Contracts.Requests;

public sealed class LoginRequest
{
    [Required(ErrorMessage = "Login is required!")]
    public string Login { get; init; }
        
    [Required(ErrorMessage = "Password is required!!")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Invalid password length")]
    public string Password { get; init; }
        
    public DateTimeOffset LastRequestTime { get; init; }
}