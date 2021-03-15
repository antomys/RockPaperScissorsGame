using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Interfaces;

namespace Server.Models
{
    [Table("Account")]
    public class Account : IAccount
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; init; }
        
        [StringLength(20)]
        [Required]
        public string Login { get; set; }
        
        [Required]
        [StringLength(20, MinimumLength=5, ErrorMessage = "Invalid password length")]
        public string Password { get; set; }
        
        [ForeignKey("StatisticsId")]
        public Statistics Statistics { get; set; }

        public Account(string login, string password)
        {
            Id = Guid.NewGuid().ToString();
            Login = login;
            Password = password;
            Statistics = new Statistics(login);
        }

    }
}