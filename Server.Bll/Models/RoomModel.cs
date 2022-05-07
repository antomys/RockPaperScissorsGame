namespace Server.Bll.Models;

public sealed class RoomModel
{
    /// <summary>
    /// Id of the room. Consists of 5 randomized chars
    /// </summary>
    public string Id { get; init; }
    
    /// <summary>
    /// Special code to join a room
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Round, linked to this room
    /// </summary>
    public RoundModel? Round { get; set; }
    
    /// <summary>
    ///     <see cref="Player"/>.
    /// </summary>
    public PlayerModel? Player { get; set; }

    /// <summary>
    /// Flag is this room is private
    /// </summary>
    public bool IsPrivate { get; set; }

    /// <summary>
    /// Flag if room is full
    /// </summary>
    public bool IsFull { get; set; }
        
    /// <summary>
    /// Creation date. After 5 minutes of inactivity will be deleted
    /// </summary>
    public long CreationTimeTicks { get; set; }
}