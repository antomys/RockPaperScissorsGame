namespace Server.Bll.Options;

public sealed class CleanerOptions
{
    public const string Section = "Cleaning";

    public TimeSpan CleanPeriod { get; init; }

    public TimeSpan RoomOutDateTime { get; init; }

    public TimeSpan RoundOutDateTime { get; init; }
}