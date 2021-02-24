using Newtonsoft.Json;
using Server.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.GameLogic.Models.Impl
{
    public class Round : IRound
    {
        [JsonProperty("roundId")]
        public string RoundId { get; init; }
        [JsonProperty("isFinished")]
        public bool IsFinished { get; set; }
        [JsonProperty("sessionIdNextMove")]
        public string SessionIdNextMove { get; set; } 
        [JsonProperty("nextMove")]
        public int NextMove { get; set; }   
        [JsonProperty("timeFineshed")]
        public DateTime TimeFinished { get; set; }
        [JsonProperty("winner")]
        public Account Winner { get; set; }
        [JsonProperty("loser")]
        public Account Loser { get; set; }
    }
}
