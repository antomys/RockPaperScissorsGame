using Newtonsoft.Json;

namespace Client.Models.Interfaces
{
    public class StatisticsDto
    {
        [JsonProperty("Login")]
        public string Login { get; set; }
        
        [JsonProperty("Score")]
        public int Score { get; set; }
    }
}