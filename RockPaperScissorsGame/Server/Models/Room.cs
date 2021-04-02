using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Interfaces;
namespace Server.Models
{
    [Table("Rooms")]
    public class Room : IRoom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string FirstPlayerId { get; set; }
        
        public bool IsReadyFirst { get; set; }
        public string SecondPlayerId { get; set; }
        
        public bool IsReadySecond { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreationTime { get; set; }
        
        [ForeignKey("RoundId")]
        public string RoundId { get; set; }
        public ICollection<Round> Rounds { get; set; }

    }
}
