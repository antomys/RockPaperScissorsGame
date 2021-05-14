using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Dal.Entities
{
    [Table("Rounds")]
    public class Round
    {
        public RoomPlayers RoomPlayers { get; set; }
        
        public bool IsFinished { get; set; }
        
        public DateTime TimeFinished { get; set; }
        
        public Account Winner { get; set; }

        public Account Loser { get; set; }
        
    }
}
