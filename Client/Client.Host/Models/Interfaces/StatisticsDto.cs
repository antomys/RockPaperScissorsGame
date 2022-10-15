using System.Text.Json.Serialization;

namespace Client.Host.Models.Interfaces;

public class StatisticsDto
{
    [JsonPropertyName("Login")]
    public string Login { get; set; }
        
    [JsonPropertyName("Score")]
    public int Score { get; set; }

    public override string ToString()
    {
        return $"Login: {Login} ; Score: {Score}\n";
    }
}