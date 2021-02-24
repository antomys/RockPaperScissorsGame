using Newtonsoft.Json;
using Server.GameLogic.Models.Impl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Contracts;

namespace Server.GameLogic.Models
{
        public interface IRoom
        {
            [JsonProperty("RoomId")]
            string RoomId { get; set; }
            [JsonProperty("Players")]
            ConcurrentDictionary<string, bool> Players { get; set; }
            [JsonProperty("CurrentRoundId")]
            string CurrentRoundId { get; set; }
            [JsonProperty("CreationTime")]
            DateTime CreationTime { get; set; }
            [JsonProperty("roundList")]
            List<Round> rounds { get; set; }
       }
    
}
