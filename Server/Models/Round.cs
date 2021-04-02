using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Interfaces;

namespace Server.Models
{
    [Table("Rounds")]
    public class Round : IRound
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; init; }
            
        [ForeignKey("Room")]
        public string RoomId { get; set; }
        public Room Room { get; set; }
        
        public bool IsFinished { get; set; }
        public RequiredGameMove FirstPlayerMove { get; set; }
        public RequiredGameMove SecondPlayerMove { get; set; }
        public DateTime TimeFinished { get; set; }
        public string WinnerId { get; set; }
        public string LoserId { get; set; }
        
    }
}
