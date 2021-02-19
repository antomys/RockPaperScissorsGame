using System;
using System.Text.Json.Serialization;

namespace RockPaperScissors.Server.Models.Interfaces
{
    public interface IAccount
    {
        [JsonPropertyName("Id")] 
        Guid Id { get; }
        
        [JsonPropertyName("Login")]
        string Login { get; set; }
        
        [JsonPropertyName("Password")]
        
        string Password { get; set; }

        Statistics Statistics { get; }
        
        //Guid StatId { get; }



    }
}