using System.Text.Json.Serialization;

namespace RockPaperScissors.Common.Responses;

public sealed class AllStatisticsResponse
{
    [JsonPropertyName("login")]
    public string Login { get; init; }

    /// <summary>
    ///     Total amount of Points. 1 win = 4 points. 1 lose = -2 points.
    /// </summary>
    [JsonPropertyName("score")]
    public int Score { get; init; }
}