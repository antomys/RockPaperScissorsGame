using System;
using System.Text.Json.Serialization;

namespace RockPaperScissors.Models.Interfaces
{
    internal interface IPersonalStatistics
    {
        IAccount Account { get; }
        
        [JsonPropertyName("Wins")]
        int Wins { get; set; }
        [JsonPropertyName("Loss")]
        int Loss { get; set; }
        [JsonPropertyName("WinLossRatio")]
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
    }
}