using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Dal.Entities;

[Table("Statistics")]
public class Statistics
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; }
    
    [ForeignKey("Account")]
    public string AccountId { get; set; }
    
    public virtual Account Account { get; set; }
    
    public int Wins { get; set; }
    
    public int Loss { get; set; }
    
    public int Draws { get; set; }
    
    public double WinLossRatio { get; set; }
    
    public TimeSpan TimeSpent { get; set; }
    
    public int UsedRock { get; set; }
    
    public int UsedPaper { get; set; }
    
    public int UsedScissors { get; set; }
    
    public int Score { get; set; }
}