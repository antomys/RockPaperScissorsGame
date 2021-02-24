using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class Room 
    {
        [JsonProperty("RoomId")]
        public string RoomId { get; set; }
        
        [JsonProperty("Players")]
        public ConcurrentDictionary<string, bool> Players { get; set; }
        
        [JsonProperty("CurrentRoundId")]
        public string CurrentRoundId { get; set; }
        
        [JsonProperty("CreationTime")]
        public DateTime CreationTime { get; set; }
    }
}
