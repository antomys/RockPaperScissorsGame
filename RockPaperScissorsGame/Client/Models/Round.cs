using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using Client.Models.Interfaces;

namespace Client.Models
{
    internal class Round : IRound
    {
        [JsonProperty("Id")]
        public string Id { get; init; }  //Not to store identical rounds
        
        [JsonProperty("Moves")]
        public ConcurrentDictionary<string, int>  PlayerMoves { get; set; } //where string key is playerId
        
        [JsonProperty("IsFinished")]
        public bool IsFinished { get; set; }  //Probably not needed.
        
        [JsonProperty("TimeFinished")]
        public DateTime TimeFinished { get; set; }
        
        [JsonProperty("WinnerId")]
        public string WinnerId { get; set; }
        
        [JsonProperty("LoserId")]
        public string LoserId { get; set; }
    }
}