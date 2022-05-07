using System.Text.Json.Serialization;

namespace Server.Host.Contracts;

public sealed class StatisticsDto
{
    [JsonPropertyName("Login")]
    public string Login { get; set; }
        
    [JsonPropertyName("Score")]
    public int Score { get; set; }
}