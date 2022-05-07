
namespace Server.Bll.Models;

public sealed class PlayerModel
{
    public PlayerModel(
        string id,
        string accountId,
        int move)
    {
        Id = id;
        AccountId = accountId;
        Move = move;
    }

    public string Id { get; init; }
    
    public string AccountId { get; set; }

    public int Move { get; set; }
}