using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Dal.Entities
{
    public class Room
    {
        public Round Round { get; set; }

        public RoomPlayers RoomPlayers { get; set; }
       
        public bool IsPrivate { get; set; }
        
        public bool IsReady { get; set; }  
        
        public bool IsFull { get; set; }
        
        public DateTime CreationTime { get; set; }  
        
        public bool IsRoundEnded { get; set; }
        
    }
}
