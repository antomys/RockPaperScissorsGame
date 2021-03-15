using System;
using System.Collections.Concurrent;
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
        public string SecondPlayerId { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsReady { get; set; }
        public bool IsFull { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsRoundEnded { get; set; }
        
        [ForeignKey("RoundId")]
        public string RoundId { get; set; }
        public ICollection<Round> Rounds { get; set; }
        
    }
}
