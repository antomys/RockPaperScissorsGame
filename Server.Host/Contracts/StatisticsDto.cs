using System.Text.Json.Serialization;

namespace Server.Host.Contracts;

public sealed class StatisticsDto
{
    [JsonPropertyName("Login")]
    public string Login { get; init; }
        
    [JsonPropertyName("Score")]
    public int Score { get; init; }
}