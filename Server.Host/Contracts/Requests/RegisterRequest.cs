using System.ComponentModel.DataAnnotations;

namespace Server.Host.Contracts.Requests;

public sealed class RegisterRequest
{
    [Required(ErrorMessage = "Login is required!")]
    public string Login { get; set; }
        
    [Required(ErrorMessage = "Password is required!")]
    [StringLength(20, MinimumLength=6, ErrorMessage = "Invalid password length. Must be 6-20")]
    public string Password { get; set; }
}