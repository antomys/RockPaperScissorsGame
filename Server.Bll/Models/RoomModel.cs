using System;

namespace Server.Bll.Models
{
    public class RoomModel
    {
        public RoundModel Round { get; set; }

        public RoomPlayersModel RoomPlayers { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsReady { get; set; }

        public bool IsFull { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsRoundEnded { get; set; }
    }
}