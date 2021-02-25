using System;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace Server.GameLogic.Models
{
    public interface IRound
    {
        [JsonPropertyName("Id")]
        string Id { get; init; }  //Not to store identical rounds
        
        [JsonPropertyName("Moves")]
        public ConcurrentDictionary<string, RequiredGameMove>  PlayerMoves { get; set; } //where string key is playerId
        
        [JsonPropertyName("IsFinished")]
        bool IsFinished { get; set; }  //Probably not needed.
        
        [JsonPropertyName("TimeFinished")]
        DateTime TimeFinished { get; set; }
        
        [JsonPropertyName("WinnerId")]
        string WinnerId { get; set; }
        
        [JsonPropertyName("LoserId")]
        string LoserId { get; set; }
        public bool IsDraw { get; set; }
    }
}
