using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
namespace Client.Models
{
    public class Room 
    {
        [JsonProperty("RoomId")]
        public string RoomId { get; set; }
        
        [JsonProperty("Players")]
        public ConcurrentDictionary<string, bool> Players { get; set; }
        
        [JsonProperty("CurrentRoundId")]
        public string CurrentRoundId { get; set; }
        
        [JsonProperty("CreationTime")]
        public DateTime CreationTime { get; set; }
        
        public bool IsReady { get; set; }
    }
}
