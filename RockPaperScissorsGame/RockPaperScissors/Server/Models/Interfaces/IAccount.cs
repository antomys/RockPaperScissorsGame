using System;
using System.Text.Json.Serialization;

namespace RockPaperScissors.Server.Models.Interfaces
{
    internal interface IAccount
    {
        [JsonPropertyName("Id")] 
        Guid Id { get; }
        
        [JsonPropertyName("Login")]
        string Login { get; set; }
        
        [JsonPropertyName("Password")]
        
        string Password { get; set; }

        IStatistics Statistics { get; }



    }
}