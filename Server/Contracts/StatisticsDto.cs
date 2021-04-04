using Newtonsoft.Json;

namespace Server.Contracts
{
    public class StatisticsDto
    {
        [JsonProperty("Login")]
        public string Login { get; set; }
        
        [JsonProperty("Score")]
        public int Score { get; set; }
    }
}