using System;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using Client.Host.Models.Interfaces;

namespace Client.Host.Models;

public class Room : IRoom
{
    [JsonPropertyName("RoomId")]
    public string RoomId { get; set; }
        
    [JsonPropertyName("Players")]
    public ConcurrentDictionary<string, bool> Players { get; set; }
        
    [JsonPropertyName("CurrentRoundId")]
    public string CurrentRoundId { get; set; }
        
    [JsonPropertyName("CreationTime")]
    public DateTime CreationTime { get; set; }
        
    public bool IsReady { get; set; }
}