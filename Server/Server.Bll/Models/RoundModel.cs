namespace Server.Bll.Models;

public sealed class RoundModel
{
    public string Id { get; init; }

    public bool IsFinished { get; init; }

    public long StartTimeTicks { get; init; }

    public long FinishTimeTicks { get; init; }

    public long UpdateTicks { get; init; }
}