using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Dal.Entities
{
    [Table("Rounds")]
    public class Round
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; init; }
        
        public int RoomPlayersId { get; set; }
        
        [ForeignKey("RoomPlayersId")]
        public virtual RoomPlayers RoomPlayers { get; set; }
        
        public bool IsFinished { get; set; }
        
        public DateTime TimeFinished { get; set; }
        
        public string WinnerId { get; set; }
        
        [ForeignKey("WinnerId")]
        public virtual Account Winner { get; set; }
        
        public string LoserId { get; set; }
        
        [ForeignKey("LoserId")]
        public virtual Account Loser { get; set; }
        
    }
}
