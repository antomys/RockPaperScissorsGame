using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Dal.Entities;

[Table("Rounds")]
public class Round
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [ForeignKey("Room")]
    public int RoomId { get; set; }
    
    public virtual Room Room { get; set; }
    
    public virtual ICollection<Player> Players { get; set; }
    
    public virtual Account Winner { get; set; }
    
    public virtual Account Loser { get; set; }

    public bool IsFinished { get; set; }
}