using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace Server.Game.Models.Interfaces
{
    public interface IRoom
    {
        [JsonProperty("RoomId")]
        string RoomId { get; set; }
        
        [JsonProperty("Players")]
        ConcurrentDictionary<Tuple<string,string>,bool> Players { get; set; }
        
        [JsonProperty("CurrentRound")]
        Round CurrentRound { get; set; }
        
        [JsonProperty("IsRoundEnded")]
        public bool IsRoundEnded { get; set; }
        
        [JsonProperty("CreationTime")]
        DateTime CreationTime { get; set; } 
        
    }
}