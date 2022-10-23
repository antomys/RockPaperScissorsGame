
namespace Server.Bll.Models;

public sealed class PlayerModel
{
    public string Id { get; init; }

    public int Move { get; init; }
    
    public bool IsReady { get; init; }

    public bool IsWinner { get; init; }
}