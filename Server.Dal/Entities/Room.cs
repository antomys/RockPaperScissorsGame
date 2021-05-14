using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Dal.Entities
{
    [Table("Rooms")]
    public class Room
    {
        /// <summary>
        /// Id of the room. Consists of 5 randomized chars
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string RoomId { get; set; }
        
        /// <summary>
        /// Id of current round
        /// </summary>
        public string RoundId { get; set; }
        
        /// <summary>
        /// Round, linked to this room
        /// </summary>
        [ForeignKey("RoundId")]
        public virtual Round Round { get; set; }
        
        public int RoomPlayerId { get; set; }
        
        [ForeignKey("RoomPlayerId")]
        public virtual RoomPlayers RoomPlayers { get; set; }
        
        /// <summary>
        /// Flag is this room is private
        /// </summary>
        public bool IsPrivate { get; set; }
        
        /// <summary>
        /// Flag if everyone in this room is ready
        /// </summary>
        public bool IsReady { get; set; }  
        
        /// <summary>
        /// Flag if room is full
        /// </summary>
        public bool IsFull { get; set; }
        
        /// <summary>
        /// Creation date. After 5 minutes of inactivity will be deleted
        /// </summary>
        public DateTime CreationTime { get; set; }  
        
        /// <summary>
        /// Flag is current count has ended
        /// </summary>
        public bool IsRoundEnded { get; set; }
        
    }
}
