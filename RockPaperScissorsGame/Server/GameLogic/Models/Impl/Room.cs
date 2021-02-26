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
        /// ConcurrentDictionary of all players
        /// </summary>
        public ConcurrentDictionary<string, bool> Players { get; set; } 
        public string CurrentRoundId { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsReady { get; set; }  
        public bool IsFull { get; set; }
        public DateTime CreationTime { get; set; }  
        public bool IsRoundEnded { get; set; }
        
    }
}
