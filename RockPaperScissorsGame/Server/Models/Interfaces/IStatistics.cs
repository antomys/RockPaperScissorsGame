using System;
using System.Text.Json.Serialization;

namespace Server.Models.Interfaces
{
    public interface IStatistics
    {
        [JsonPropertyName("Id")]
        string Id { get; set; }
        
        [JsonPropertyName("UserLogin")]
        string Login { get; set; }
        
        [JsonPropertyName("Wins")]
        int Wins { get; set; }
        
        [JsonPropertyName("Loss")]
        int Loss { get; set; }
        
        [JsonPropertyName("WinToLossRatio")]
        double WinLossRatio { get; set; }
        
        [JsonPropertyName("TimeSpent")]
        string TimeSpent { get; set; }
        
        [JsonPropertyName("UsedRock")]
        int UsedRock { get; set; }
        
        [JsonPropertyName("UsedPaper")]
        int UsedPaper { get; set; }
        
        [JsonPropertyName("UsedScissors")]
        int UsedScissors { get; set; }
        
        [JsonPropertyName("Score")]
        int Score { get; set; }

        //IAccount Account { get; }
    }
}