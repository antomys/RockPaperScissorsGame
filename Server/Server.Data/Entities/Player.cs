using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data.Entities;

[Table(nameof(Player))]
public class Player
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; init; }
    
    [ForeignKey(nameof(Account))]
    public string AccountId { get; init; }
    
    public virtual Account Account { get; set; }
    
    public bool IsReady { get; set; }
    
    public int Move { get; set; }
    
    public bool IsWinner { get; set; }
}