using System;
using System.Collections.Concurrent;

namespace Server.GameLogic.Models.Impl
{
    public class Room : IRoom
    {
        /// <summary>
        /// Id of the room. Consists of 5 randomized chars
        /// </summary>
        public string RoomId { get; set; }
        
        /// <summary>
        /// ConcurrentDictionary of all players.
        /// Where string - account Id. Bool - flag is he ready
        /// </summary>
        public ConcurrentDictionary<string, bool> Players { get; set; } 
        
        /// <summary>
        /// Id of current round
        /// </summary>
        public string CurrentRoundId { get; set; }
        
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
