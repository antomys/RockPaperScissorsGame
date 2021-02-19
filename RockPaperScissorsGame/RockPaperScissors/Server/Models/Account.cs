using System;
using RockPaperScissors.Server.Models.Interfaces;
using RockPaperScissors.Server.Services;

namespace RockPaperScissors.Server.Models
{
    public class Account : IAccount
    {
        public Guid Id { get; set; }

        public string Login { get; set; }
        
        public string Password { get; set; }

       // public Guid StatId { get; set; }
       //public Statistics Statistics { get; set; }
    }
}