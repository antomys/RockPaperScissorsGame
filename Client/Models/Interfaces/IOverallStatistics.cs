namespace Client.Models.Interfaces;

public interface IOverallStatistics
{
    Account Account { get; set; }
    
    int Score { get; set; }
}