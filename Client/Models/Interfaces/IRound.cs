using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace Client.Models.Interfaces;

internal interface IRound
{
    [JsonProperty("Id")]
    string Id { get; init; }  //Not to store identical rounds
        
    [JsonProperty("Moves")]
    ConcurrentDictionary<string, int>  PlayerMoves { get; set; } //where string key is playerId
        
    [JsonProperty("IsFinished")]
    bool IsFinished { get; set; }  //Probably not needed.
        
    [JsonProperty("TimeFinished")]
    DateTime TimeFinished { get; set; }
        
    [JsonProperty("WinnerId")]
    string WinnerId { get; set; }
        
    [JsonProperty("LoserId")]
    string LoserId { get; set; }
}