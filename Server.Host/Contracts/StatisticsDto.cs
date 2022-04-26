using Newtonsoft.Json;

namespace Server.Host.Contracts;

public sealed class StatisticsDto
{
    [JsonProperty("Login")]
    public string Login { get; set; }
        
    [JsonProperty("Score")]
    public int Score { get; set; }
}