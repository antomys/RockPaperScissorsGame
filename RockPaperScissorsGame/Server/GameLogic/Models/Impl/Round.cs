using System;
using System.Collections.Concurrent;

namespace Server.GameLogic.Models.Impl
{
    public class Round : IRound
    {
        /// <summary>
        /// Id of current round. GUID
        /// </summary>
        public string Id { get; init; }
        
        /// <summary>
        /// Id of room, Where this round is situated. OBSOLETE
        /// </summary>

        public string RoomId { get; set; }
        
        /// <summary>
        /// Flag is this round has ended
        /// </summary>
        public bool IsFinished { get; set; }
        
        /// <summary>
        /// Dictionary of player Id and his move
        /// </summary>
        public ConcurrentDictionary<string, RequiredGameMove>  PlayerMoves { get; set; }
        
        /// <summary>
        /// Is the result is draw
        /// </summary>
        public bool IsDraw { get; set; }
        
        /// <summary>
        /// Time of finishing this room. Also used to check 20 seconds for the move.
        /// </summary>
        public DateTime TimeFinished { get; set; }
        
        /// <summary>
        /// Login of winner Id
        /// </summary>
        public string WinnerId { get; set; }
        
        /// <summary>
        /// Login of loser Id
        /// </summary>
        public string LoserId { get; set; }
        
    }
}
