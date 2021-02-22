using System;
using Newtonsoft.Json;

namespace Server.Models
{
    public class Rounds
    {
        [JsonProperty("TimeEnded")]
        public TimeSpan TimeEnded { get; set; }
        
        [JsonProperty("IsEnded")]
        public bool IsEnded { get; set; }
        
        [JsonProperty("Winner")]
        public Account Winner { get; set; }
        
        [JsonProperty("Loser")]
        public Account Loser { get; set; }
    }
}