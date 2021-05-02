using System;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace Server.GameLogic.Models.Interfaces
{
    public interface IRound
    {
        [JsonPropertyName("Id")]
        string Id { get; init; } 
        
        [JsonPropertyName("RoomId")]
        string RoomId { get; set; }
        
        [JsonPropertyName("Moves")]
        public ConcurrentDictionary<string, RequiredGameMove>  PlayerMoves { get; set; } 
        
        [JsonPropertyName("IsFinished")]
        bool IsFinished { get; set; } 
        
        [JsonPropertyName("TimeFinished")]
        DateTime TimeFinished { get; set; }
        
        [JsonPropertyName("WinnerId")]
        string WinnerId { get; set; }
        
        [JsonPropertyName("LoserId")]
        string LoserId { get; set; }
        public bool IsDraw { get; set; }
    }
}
