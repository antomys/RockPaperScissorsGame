using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data.Entities;

[Table(nameof(Round))]
public class Round
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; init; }

    [ForeignKey(nameof(Room))]
    public string RoomId { get; set; }
    
    public virtual Room Room { get; set; }
    
    public virtual ICollection<Player> Players { get; set; }

    public bool IsFinished { get; init; }
    
    public long StartTimeTicks { get; set; }
    
    public long FinishTimeTicks { get; set; }
    
    public long UpdateTicks { get; set; }
}