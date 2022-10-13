using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data.Entities;

[Table("Rounds")]
public class Round
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; init; }

    [ForeignKey("Room")]
    public string RoomId { get; set; }
    
    public virtual Room Room { get; set; }
    
    public virtual ICollection<Player> Players { get; set; }
    
    public virtual Account Winner { get; set; }
    
    public virtual Account Loser { get; set; }
    
    public long StartTimeTicks { get; set; }
    
    public long FinishTimeTicks { get; set; }

    public bool IsFinished { get; init; }
}