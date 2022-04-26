using System;

namespace Server.Bll.Models;

public sealed class RoomModel
{
    public int Id { get; set; }
        
    public int? RoundId { get; set; }
        
    public string RoomCode { get; set; }

    public RoomPlayersModel RoomPlayers { get; set; }

    public bool IsPrivate { get; set; }

    public bool IsReady { get; set; }

    public bool IsFull { get; set; }

    public DateTimeOffset CreationTime { get; set; }

    public bool IsRoundEnded { get; set; }
}