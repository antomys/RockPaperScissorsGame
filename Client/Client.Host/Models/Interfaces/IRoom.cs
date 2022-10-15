using System;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace Client.Host.Models.Interfaces;

internal interface IRoom
{
 [JsonPropertyName("RoomId")]
 string RoomId { get; set; }
        
 [JsonPropertyName("Players")]
 ConcurrentDictionary<string, bool> Players { get; set; }
        
 [JsonPropertyName("CurrentRoundId")]
 string CurrentRoundId { get; set; }
        
 [JsonPropertyName("CreationTime")]
 DateTime CreationTime { get; set; } 
 bool IsReady { get; set; }
}