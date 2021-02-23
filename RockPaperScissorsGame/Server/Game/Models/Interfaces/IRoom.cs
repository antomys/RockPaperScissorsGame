using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace Server.Game.Models.Interfaces
{
    public interface IRoom
    {
        [JsonProperty("RoomId")]
        string RoomId { get; set; }
        
        [JsonProperty("NextMove")]
        string NextMove { get; set; }   //Idea to store SessionId of user, that has to make move.
        
        [JsonProperty("Players")]
        ConcurrentDictionary<Tuple<string,string>,object> Players { get; set; }
        
        [JsonProperty("IsRoundEnded")]
        public bool IsRoundEnded { get; set; }
        
    }
}