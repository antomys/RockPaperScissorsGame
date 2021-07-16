using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Server.Dal.Entities
{
    [Table("RoomPlayers")]
    public class RoomPlayers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        public int? FirstPlayerId { get; set; }
        [ForeignKey("FirstPlayerId")]
        public virtual Account FirstPlayer { get; set; }
        public int? SecondPlayerId { get; set; }
        [ForeignKey("SecondPlayerId")]
        public virtual Account SecondPlayer { get; set; }
        public int? RoundId { get; set; }
        [ForeignKey("RoundId")]
        public virtual Round Round { get; set; }
        
    }
}