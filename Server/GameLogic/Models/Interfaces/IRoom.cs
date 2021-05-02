using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace Server.GameLogic.Models.Interfaces
{
        public interface IRoom
        {
            [JsonProperty("RoomId")]
            string RoomId { get; set; }
            [JsonProperty("Players")]
            ConcurrentDictionary<string, bool> Players { get; set; }
            [JsonProperty("CurrentRoundId")]
            string CurrentRoundId { get; set; }
            [JsonProperty("CreationTime")]
            DateTime CreationTime { get; set; }
        }
    
}
