using System.Collections.Generic;

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
    public string Code { get; init; }

    /// <summary>
    /// Round, linked to this room
    /// </summary>
    public RoundModel? Round { get; init; }

    /// <summary>
    ///     <see cref="PlayerModel"/>.
    /// </summary>
    public ICollection<PlayerModel>? Players { get; init; }

    /// <summary>
    /// Flag is this room is private
    /// </summary>
    public bool IsPrivate { get; init; }

    /// <summary>
    /// Flag if room is full
    /// </summary>
    public bool IsFull { get; init; }
        
    /// <summary>
    /// Creation date. After 5 minutes of inactivity will be deleted
    /// </summary>
    public long CreationTimeTicks { get; init; }
    
    /// <summary>
    ///     Last update time ticks.
    /// </summary>
    public long UpdateTicks { get; init; }
}