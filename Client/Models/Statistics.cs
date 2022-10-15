using Client.Models.Interfaces;

namespace Client.Models;

public class Statistics : IStatistics, IOverallStatistics
{
        
    /// <summary>
    /// user 
    /// </summary>
    public Account Account { get; set; }
    /// <summary>
    ///     Total amount of Wins
    /// </summary>
    public int Wins { get; set; }

    /// <summary>
    ///     Total amount of Loses
    /// </summary>
    public int Loss { get; set; }

    /// <summary>
    ///     Total amount of Draws. OBSOLETE
    /// </summary>

    public int Draws { get; set; }

    /// <summary>
    ///     Ratio Wins to Losses. Win/Loss * 100
    /// </summary>
    public double WinLossRatio { get; set; }

    /// <summary>
    ///     Ratio for the last 7 days
    /// </summary>
    public string TimeSpent { get; set; }

    /// <summary>
    ///     Times used rock
    /// </summary>
    public int UsedRock { get; set; }

    /// <summary>
    ///     Times used Paper
    /// </summary>
    public int UsedPaper { get; set; }

    /// <summary>
    ///     Times used Scissors
    /// </summary>
    public int UsedScissors { get; set; }

    /// <summary>
    ///     Total amount of Points. 1 win = 4 points. 1 lose = -2 points.
    /// </summary>
    public int Score { get; set; }

    public override string ToString()
    {
        return $"Times Won: {Wins}\n" +
               $"Times Lost:{Loss}\n" +
               $"Win to Loss ratio: {WinLossRatio}\n" +
               $"{TimeSpent}\n" +
               $"Times used rock: {UsedRock}\n" +
               $"Times used paper: {UsedPaper}\n" +
               $"Times used scissors: {UsedScissors}\n" + 
               $"Total score: {Score}";
    }

    public string ToShortString()
    {
        return $"UserName: {Account.Login}" +
               $"Score: {Score}";
    }
}