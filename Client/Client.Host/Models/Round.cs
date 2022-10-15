using System;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using Client.Host.Models.Interfaces;

namespace Client.Host.Models;

internal class Round : IRound
{
    [JsonPropertyName("Id")]
    public string Id { get; init; }  //Not to store identical rounds
        
    [JsonPropertyName("Moves")]
    public ConcurrentDictionary<string, int>  PlayerMoves { get; set; } //where string key is playerId
        
    [JsonPropertyName("IsFinished")]
    public bool IsFinished { get; set; }  //Probably not needed.
        
    [JsonPropertyName("TimeFinished")]
    public DateTime TimeFinished { get; set; }
        
    [JsonPropertyName("WinnerId")]
    public string WinnerId { get; set; }
        
    [JsonPropertyName("LoserId")]
    public string LoserId { get; set; }
        
    [JsonPropertyName("IsDraw")]
    public bool IsDraw { get; set; }
}