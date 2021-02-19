using System;
using RockPaperScissors.Server.Models.Interfaces;

namespace RockPaperScissors.Server.Models
{
    public class Statistics : IStatistics
    {
        public Guid Id { get; set; }
        public Guid Userid { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public double WinLossRatio { get; set; }
        public TimeSpan TimeSpent { get; set; }
        public int UsedRock { get; set; }
        public int UsedPaper { get; set; }
        public int UsedScissors { get; set; }
        public int Points { get; set; }
        //public IAccount Account { get; set; }
    }
}