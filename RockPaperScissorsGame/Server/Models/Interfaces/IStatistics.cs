using Newtonsoft.Json;

namespace Server.Models.Interfaces
{
    public interface IStatistics
    {
        [JsonProperty("Id")]
        string Id { get; set; }
        
        [JsonProperty("UserLogin")]
        string Login { get; set; }
        
        [JsonProperty("Wins")]
        int Wins { get; set; }
        
        [JsonProperty("Loss")]
        int Loss { get; set; }
        
        [JsonProperty("WinToLossRatio")]
        double WinLossRatio { get; set; }
        
        [JsonProperty("TimeSpent")]
        string TimeSpent { get; set; }  //This has to get statistics of games from last 7 days.
        
        [JsonProperty("UsedRock")]
        int UsedRock { get; set; }
        
        [JsonProperty("UsedPaper")]
        int UsedPaper { get; set; }
        
        [JsonProperty("UsedScissors")]
        int UsedScissors { get; set; }
        
        [JsonProperty("Score")]
        int Score { get; set; }
        
    }
}