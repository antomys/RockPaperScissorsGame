namespace Server.Bll.Models;

public sealed class StatisticsModel
{
    /// <summary>
    ///     Total amount of Wins
    /// </summary>
    public int? Wins { get; init; }

    /// <summary>
    ///     Total amount of Loses
    /// </summary>
    public int? Loss { get; init; }

    /// <summary>
    ///     Total amount of Draws. OBSOLETE
    /// </summary>

    public int? Draws { get; init; }

    /// <summary>
    ///     Ratio Wins to Losses. Win/Loss * 100
    /// </summary>
    public double? WinLossRatio { get; init; }

    /// <summary>
    ///     Ratio for the last 7 days
    /// </summary>
    public string TimeSpent { get; init; }

    /// <summary>
    ///     Times used rock
    /// </summary>
    public int? UsedRock { get; init; }

    /// <summary>
    ///     Times used Paper
    /// </summary>
    public int? UsedPaper { get; init; }

    /// <summary>
    ///     Times used Scissors
    /// </summary>
    public int? UsedScissors { get; init; }

    /// <summary>
    ///     Total amount of Points. 1 win = 4 points. 1 lose = -2 points.
    /// </summary>
    public int? Score { get; init; }
}