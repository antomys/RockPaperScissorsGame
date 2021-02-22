using System.ComponentModel.DataAnnotations;
using Server.Models.Interfaces;

namespace Server.Models
{
    public class Account : IAccount
    {
        public string Id { get; init; }

        public string Login { get; set; }

        public string Password { get; set; }

       // public Guid StatId { get; set; }
       //public Statistics Statistics { get; set; }
    }
}