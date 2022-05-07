using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Dal.Entities;

[Table("Players")]
public class Player
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    [ForeignKey("Account")]
    public int AccountId { get; set; }
    
    public virtual Account Account { get; set; }
    
    public int Move { get; set; }
}