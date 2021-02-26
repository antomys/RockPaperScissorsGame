using System.ComponentModel.DataAnnotations;
using Server.Models.Interfaces;

namespace Server.Models
{
    public class Account : IAccount
    {
        
        /// <summary>
        /// Id of account. Unique to everyone and similar with Statistics Id
        /// </summary>
        public string Id { get; init; }
        
        /// <summary>
        /// Nick name of Account
        /// </summary>
        public string Login { get; set; }
        
        /// <summary>
        /// Password of the Account
        /// </summary>
        [StringLength(20, MinimumLength=5, ErrorMessage = "Invalid password length")]
        public string Password { get; set; }

    }
}