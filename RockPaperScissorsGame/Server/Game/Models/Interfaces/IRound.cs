using System;
using Newtonsoft.Json;
using Server.Models;

namespace Server.Game.Models.Interfaces
{
    public interface IRound
    { 
        [JsonProperty("RoundId")]
        string RoundId { get; init; }  //Not to store identical rounds
       
        [JsonProperty("SessionIdNextMove")]
        public string SessionIdNextMove { get; set; }   //Idea to store SessionId of user, that has to make move.
        
        [JsonProperty("NextMove")]
        public int NextMove { get; set; }   //enum
        
        [JsonProperty("IsFinished")]
        bool IsFinished { get; set; }  //Probably not needed.
        
        [JsonProperty("TimeFinisher")]
        DateTime TimeFinished { get; set; }
        
        [JsonProperty("Winner")]
        Account Winner { get; set; }
        
        [JsonProperty("Loser")]
        Account Loser { get; set; } 
    }
}