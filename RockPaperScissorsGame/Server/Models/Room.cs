using System.Collections.Generic;

namespace Server.Models
{
    public class Room
    {
        public string RoomId { get; init; }
        
        public bool IsFull { get; set; }

        public List<Account> Players { get; init; }
        
        public Rounds Rounds { get; set; }
    }
}