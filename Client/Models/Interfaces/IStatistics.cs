using Newtonsoft.Json;

namespace Client.Models.Interfaces
{
    internal interface IStatistics
    {
        [JsonProperty("Id")]
        string Id { get; set; }
        
        [JsonProperty("UserLogin")]
        string Login { get; set; }
        
        [JsonProperty("Wins")]
        int Wins { get; set; }
        
        [JsonProperty("Loss")]
        int Loss { get; set; }
        
        [JsonProperty("Draws")]
        int Draws { get; set; }
        
        [JsonProperty("WinToLossRatio")]
        double WinLossRatio { get; set; }
        
        [JsonProperty("TimeSpent")]
        string TimeSpent { get; set; }
        
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