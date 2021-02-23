using System;
using System.Collections.Concurrent;
using Server.Game.Models.Interfaces;

namespace Server.Game.Models
{
    public class Room : IRoom
    {
        public string RoomId { get; set; }

        public ConcurrentDictionary<Tuple<string,string>,bool> Players { get; set; } //Where string - sessionId or login of player, bool - is he ready to play
                                                                        //Temporary made tuple to store sessionId and login.
        public Round CurrentRound { get; set; }
        
        public bool IsPrivate { get; set; }

        public bool IsReady { get; set; }  //To start game

        public bool IsFull { get; set; }
        
        public DateTime CreationTime { get; set; }  //this to check 5 minutes and then delete room
        
        public bool IsRoundEnded { get; set; }
    }
}