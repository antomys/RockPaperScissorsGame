using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace Client.Models.Interfaces;

internal interface IRoom
{
 [JsonProperty("RoomId")]
 string RoomId { get; set; }
        
 [JsonProperty("Players")]
 ConcurrentDictionary<string, bool> Players { get; set; }
        
 [JsonProperty("CurrentRoundId")]
 string CurrentRoundId { get; set; }
        
 [JsonProperty("CreationTime")]
 DateTime CreationTime { get; set; } 
 bool IsReady { get; set; }
}