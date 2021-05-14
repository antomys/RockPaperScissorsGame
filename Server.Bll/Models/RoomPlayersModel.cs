using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Server.Dal.Entities
{
    [Table("RoomPlayers")]
    public class RoomPlayers
    {
        public Room Room { get; set; }
        
        public ICollection<Account> Accounts { get; set; }
        
        public int FirstPlayerMove { get; set; }
        
        public int SecondPlayerMove { get; set; }

        public string RoundId { get; set; }

        public Round Round { get; set; }
    }
}