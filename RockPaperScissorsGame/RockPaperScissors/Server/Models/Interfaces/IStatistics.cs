using System;
using System.Text.Json.Serialization;

namespace RockPaperScissors.Server.Models.Interfaces
{
    internal interface IStatistics
    {
        [JsonPropertyName("Id")]
        Guid Id { get; set; }
        
        [JsonPropertyName("UserId")]
        Guid Userid { get; set; }
        
        [JsonPropertyName("Wins")]
        int Wins { get; set; }
        
        [JsonPropertyName("Loss")]
        int Loss { get; set; }
        
        [JsonPropertyName("WinToLossRatio")]
        double WinLossRatio { get; set; }
        
        [JsonPropertyName("TimeSpent")]
        TimeSpan TimeSpent { get; set; }
        
        [JsonPropertyName("UsedRock")]
        int UsedRock { get; set; }
        
        [JsonPropertyName("UsedPaper")]
        int UsedPaper { get; set; }
        
        [JsonPropertyName("UsedScissors")]
        int UsedScissors { get; set; }
        
        [JsonPropertyName("Points")]
        int Points { get; set; }

        IAccount Account { get; }
    }
}