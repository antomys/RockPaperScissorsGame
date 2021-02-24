using Newtonsoft.Json;
using System;

namespace Client.Models
{
    public class Round
    {
        [JsonProperty("roundId")]
        public string RoundId { get; init; }
        [JsonProperty("isFinished")]
        public bool IsFinished { get; set; }
        [JsonProperty("sessionIdNextMove")]
        public string SessionIdNextMove { get; set; } //For confirmation next game
        [JsonProperty("nextMove")]
        public int NextMove { get; set; }             //For confirmation next game
        [JsonProperty("timeFineshed")]
        public DateTime TimeFinished { get; set; }
        [JsonProperty("winner")]
        public Account Winner { get; set; }
        [JsonProperty("loser")]
        public Account Loser { get; set; }
    }
}