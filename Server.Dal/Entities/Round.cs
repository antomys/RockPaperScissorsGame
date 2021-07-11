using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Dal.Entities
{
    [Table("Rounds")]
    public class Round
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; init; }
        
        public int RoomPlayersId { get; set; }
        
        [ForeignKey("RoomPlayersId")]
        public virtual RoomPlayers RoomPlayers { get; set; }
        
        public bool IsFinished { get; set; }
        
        public DateTimeOffset TimeFinished { get; set; }
        
        public int WinnerId { get; set; }
        
        [ForeignKey("WinnerId")]
        public virtual Account Winner { get; set; }
        
        public int LoserId { get; set; }
        
        [ForeignKey("LoserId")]
        public virtual Account Loser { get; set; }
        
    }
}
