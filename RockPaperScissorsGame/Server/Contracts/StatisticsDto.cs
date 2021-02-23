using Newtonsoft.Json;

namespace Server.Contracts
{
    public class StatisticsDto
    {
        [JsonProperty("UserLogin")]
        public string Login { get; set; }
        
        [JsonProperty("Score")]
        public int Score { get; set; }
    }
}