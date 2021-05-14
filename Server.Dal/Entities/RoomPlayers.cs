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
        public int Id { get; set; }
        
        public string RoomId { get; set; }
        
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        
        public virtual ICollection<Account> Accounts { get; set; }
        
        public int FirstPlayerMove { get; set; }
        
        public int SecondPlayerMove { get; set; }

        public string RoundId { get; set; }
        
        [ForeignKey("RoundId")]
        [AllowNull]
        public virtual Round Round { get; set; }
    }
}