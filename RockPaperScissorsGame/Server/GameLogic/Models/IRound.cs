using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;

namespace Server.GameLogic.Models
{
    public interface IRound
    {
        [JsonProperty("RoundId")]
        string RoundId { get; init; }  //Not to store identical rounds
        [JsonProperty("Moves")]
        public ConcurrentDictionary<string, int>  Moves { get; set; }
        [JsonProperty("IsFinished")]
        bool IsFinished { get; set; }  //Probably not needed.
        [JsonProperty("TimeFinished")]
        DateTime TimeFinished { get; set; }
        [JsonProperty("WinnerId")]
        string WinnerId { get; set; }
        [JsonProperty("LoserId")]
        string LoserId { get; set; }
    }
}
