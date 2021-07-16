using System;

namespace Server.Bll.Models
{
    public class RoundModel
    {
        public int Id { get; set; }
        public bool IsFinished { get; set; }
        public DateTimeOffset TimeFinished { get; set; }
        public AccountModel Winner { get; set; }
        public AccountModel Loser { get; set; }
        public int FirstPlayerMove { get; set; }
        public int SecondPlayerMove { get; set; }
    }
}