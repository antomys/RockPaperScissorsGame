namespace Client.Models.Interfaces;

public interface IStatistics
{
    /// <summary>
    /// This user account
    /// </summary>
    Account Account { get; set; }
    /// <summary>
    ///     Total amount of Wins
    /// </summary>
    int Wins { get; set; }

    /// <summary>
    ///     Total amount of Loses
    /// </summary>
    int Loss { get; set; }

    /// <summary>
    ///     Total amount of Draws. OBSOLETE
    /// </summary>

    int Draws { get; set; }

    /// <summary>
    ///     Ratio Wins to Losses. Win/Loss * 100
    /// </summary>
    double WinLossRatio { get; set; }

    /// <summary>
    ///     Ratio for the last 7 days
    /// </summary>
    string TimeSpent { get; set; }

    /// <summary>
    ///     Times used rock
    /// </summary>
    int UsedRock { get; set; }

    /// <summary>
    ///     Times used Paper
    /// </summary>
    int UsedPaper { get; set; }

    /// <summary>
    ///     Times used Scissors
    /// </summary>
    int UsedScissors { get; set; }

    /// <summary>
    ///     Total amount of Points. 1 win = 4 points. 1 lose = -2 points.
    /// </summary>
    int Score { get; set; }
        
}