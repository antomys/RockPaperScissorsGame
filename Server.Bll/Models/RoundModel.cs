using System;

namespace Server.Bll.Models
{
    public class RoundModel
    {
        public RoomPlayersModel RoomPlayers { get; set; }

        public bool IsFinished { get; set; }

        public DateTime TimeFinished { get; set; }

        public AccountModel Winner { get; set; }

        public AccountModel Loser { get; set; }
    }
}