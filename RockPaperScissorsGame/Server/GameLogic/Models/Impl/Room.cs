using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Contracts;

namespace Server.GameLogic.Models.Impl
{
    public class Room : IRoom
    {
        public string RoomId { get; set; }
        public ConcurrentDictionary<string, bool> Players { get; set; } //Where string - sessionId or login of player, bool - is he ready to play//Temporary made tuple to store sessionId and login.
        
        public string CurrentRoundId { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsReady { get; set; }  //To start game
        public bool IsFull { get; set; }
        public DateTime CreationTime { get; set; }  //this to check 5 minutes and then delete room
        public bool IsRoundEnded { get; set; }
        
        public List<Round> rounds { get; set; }
    }
}
