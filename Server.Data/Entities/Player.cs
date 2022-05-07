using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data.Entities;

[Table("Players")]
public class Player
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; init; }
    
    [ForeignKey("Account")]
    public string AccountId { get; set; }
    
    public virtual Account Account { get; set; }
    
    public bool IsReady { get; set; }
    
    public int Move { get; set; }
}