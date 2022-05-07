namespace Server.Bll.Models;

public sealed class ShortStatisticsModel
{
    public AccountModel Account { get; set; }

    /// <summary>
    ///     Total amount of Points. 1 win = 4 points. 1 lose = -2 points.
    /// </summary>
    public int? Score { get; set; }
}