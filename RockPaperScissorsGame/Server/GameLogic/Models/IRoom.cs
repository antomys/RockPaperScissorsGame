using Newtonsoft.Json;
using Server.GameLogic.Models.Impl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.GameLogic.Models
{
        public interface IRoom
        {
            [JsonProperty("RoomId")]
            string RoomId { get; set; }
            [JsonProperty("Players")]
            ConcurrentDictionary<Tuple<string, string>, bool> Players { get; set; }
            [JsonProperty("CurrentRound")]
            Round CurrentRound { get; set; }
            [JsonProperty("CreationTime")]
            DateTime CreationTime { get; set; }
            [JsonProperty("roundList")]
            List<Round> rounds { get; set; }
       }
    
}
