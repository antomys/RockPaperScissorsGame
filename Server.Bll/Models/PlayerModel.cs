
namespace Server.Bll.Models;

public sealed class PlayerModel
{
    public PlayerModel(
        string id,
        int move)
    {
        Id = id;
        Move = move;
    }

    public string Id { get; init; }

    public int Move { get; set; }
}