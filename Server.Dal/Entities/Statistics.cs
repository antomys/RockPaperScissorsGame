using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Dal.Entities
{
    [Table("Statistics")]
    public class Statistics
    {
        /// <summary>
        /// Id of statistics. Equivalent to Account Id
        /// </summary>

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        /// <summary>
        /// Id of linked account
        /// </summary>
        public int AccountId { get; set; }
        
        /// <summary>
        /// Linked account
        /// </summary>
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        
        /// <summary>
        /// Total amount of Wins
        /// </summary>
        public int Wins { get; set; }
        
        /// <summary>
        /// Total amount of Loses
        /// </summary>
        public int Loss { get; set; }
        
        /// <summary>
        /// Total amount of Draws. OBSOLETE
        /// </summary>
        
        public int Draws { get; set; }
        
        /// <summary>
        /// Ratio Wins to Losses. Win/Loss * 100
        /// </summary>
        public double WinLossRatio { get; set; }
        
        /// <summary>
        /// Ratio for the last 7 days
        /// </summary>
        public string TimeSpent { get; set; }
        
        /// <summary>
        /// Times used rock
        /// </summary>
        public int UsedRock { get; set; }
        
        /// <summary>
        /// Times used Paper
        /// </summary>
        public int UsedPaper { get; set; }
        
        /// <summary>
        /// Times used Scissors
        /// </summary>
        public int UsedScissors { get; set; }
        
        /// <summary>
        /// Total amount of Points. 1 win = 4 points. 1 lose = -2 points.
        /// </summary>
        public int Score { get; set; }
        
    }
}