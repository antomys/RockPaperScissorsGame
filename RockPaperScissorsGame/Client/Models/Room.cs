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
        [JsonProperty("roomId")]
        public string RoomId { get; set; }
        [JsonProperty("playersInLobby")]
        public ConcurrentDictionary<Tuple<string, string>, bool> Players { get; set; } //Where string - sessionId or login of player, bool - is he ready to play
        [JsonProperty("isPrivate")]
        public bool IsPrivate { get; set; }
        [JsonProperty("isReady")]
        public bool IsReady { get; set; }  //To start game
        [JsonProperty("isFull")]
        public bool IsFull { get; set; }
        [JsonProperty("creationTime")]
        public DateTime CreationTime { get; set; }  //this to check 5 minutes and then delete room
    }
}
