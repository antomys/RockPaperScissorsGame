using System.Text.Json.Serialization;

namespace RockPaperScissors.Common.Responses;

public sealed class PersonalStatisticsResponse
{
    /// <summary>
    ///     Total amount of Wins
    /// </summary>
    [JsonPropertyName("wins")]
    public int Wins { get; set; }

    /// <summary>
    ///     Total amount of Loses
    /// </summary>
    [JsonPropertyName("loss")]
    public int Loss { get; set; }

    /// <summary>
    ///     Total amount of Draws. OBSOLETE
    /// </summary>
    [JsonPropertyName("draws")]
    public int Draws { get; set; }

    /// <summary>
    ///     Ratio Wins to Losses. Win/Loss * 100
    /// </summary>
    [JsonPropertyName("winLossRatio")]
    public double WinLossRatio { get; set; }

    /// <summary>
    ///     Ratio for the last 7 days
    /// </summary>
    [JsonPropertyName("timeSpent")]
    public string TimeSpent { get; set; }

    /// <summary>
    ///     Times used rock
    /// </summary>
    [JsonPropertyName("usedRock")]
    public int UsedRock { get; set; }

    /// <summary>
    ///     Times used Paper
    /// </summary>
    [JsonPropertyName("usedPaper")]
    public int UsedPaper { get; set; }

    /// <summary>
    ///     Times used Scissors
    /// </summary>
    [JsonPropertyName("usedScissors")]
    public int UsedScissors { get; set; }

    /// <summary>
    ///     Total amount of Points. 1 win = 4 points. 1 lose = -2 points.
    /// </summary>
    [JsonPropertyName("score")]
    public int Score { get; set; }
}