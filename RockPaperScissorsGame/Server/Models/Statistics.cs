using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Interfaces;

namespace Server.Models
{
    [Table("Statistics")]
    public class Statistics : IStatistics
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string Login { get; set; }
        
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        
        public int Wins { get; set; }
        
        public int Loss { get; set; }
        
        public double WinLossRatio { get; set; }
        
        public string TimeSpent { get; set; }
        
        public int UsedRock { get; set; }
        
        public int UsedPaper { get; set; }
        
        public int UsedScissors { get; set; }
        public int Score { get; set; }

        public Statistics(string login)
        {
            Id = Guid.NewGuid().ToString();
            Login = login;
        }
    }
}