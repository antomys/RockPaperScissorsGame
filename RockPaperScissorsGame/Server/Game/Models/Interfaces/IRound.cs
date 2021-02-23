using System;
using Newtonsoft.Json;
using Server.Models;

namespace Server.Game.Models.Interfaces
{
    public interface IRound
    { 
        [JsonProperty("RoundId")]
        string RoundId { get; init; }  //Not to store identical rounds
        
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