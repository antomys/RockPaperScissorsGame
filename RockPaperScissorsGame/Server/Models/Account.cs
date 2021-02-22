using System.ComponentModel.DataAnnotations;
using Server.Models.Interfaces;

namespace Server.Models
{
    public class Account : IAccount
    {
        public string Id { get; init; }
        
        public string Login { get; set; }
        [StringLength(20, MinimumLength=6, ErrorMessage = "Invalid password length")]
        public string Password { get; set; }

    }
}