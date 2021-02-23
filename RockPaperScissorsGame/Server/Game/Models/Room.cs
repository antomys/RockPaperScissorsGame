using System;
using System.Collections.Concurrent;
using Server.Game.Models.Interfaces;

namespace Server.Game.Models
{
    public class Room : IRoom
    {
        public string RoomId { get; set; }
        
        public string NextMove { get; set; }   //Idea to store SessionId of user, that has to make move.
        
        public ConcurrentDictionary<Tuple<string,string>,object> Players { get; set; } //Where string - sessionId or login of player, bool - is he ready to play
                                                                        //Temporary made tuple to store sessionId and login.
        public Round CurrentRound { get; set; }
        
        public bool IsPrivate { get; set; }

        public bool IsReady { get; set; }  //To start game

        public bool IsFull { get; set; }
        
        public bool IsRoundEnded { get; set; }
    }
}