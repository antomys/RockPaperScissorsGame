using System.ComponentModel.DataAnnotations;

namespace Server.Host.Contracts;

public sealed class AccountDto
{
    [Required(ErrorMessage = "Login is required!")]
    public string Login { get; set; }
    
    [Required(ErrorMessage = "Password is required!!")]
    [StringLength(20, MinimumLength=6, ErrorMessage = "Invalid password length")]
    public string Password { get; set;}
}