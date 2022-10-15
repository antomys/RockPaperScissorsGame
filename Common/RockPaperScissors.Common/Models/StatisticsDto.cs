using System.Text.Json.Serialization;

namespace RockPaperScissors.Common.Models;

public sealed class StatisticsDto
{
    [JsonPropertyName("login")]
    public string Login { get; init; }
        
    [JsonPropertyName("score")]
    public int Score { get; init; }
}