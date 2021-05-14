using System.ComponentModel.DataAnnotations;

namespace Server.Contracts.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Login is required!")]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }
    }
}