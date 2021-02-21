using System;
using Server.Models.Interfaces;

namespace Server.Models
{
    public class Statistics : IStatistics
    {
        public string Id { get; set; }
        
        public string Login { get; set; }
        //public string Userid { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public double WinLossRatio { get; set; }
        public TimeSpan TimeSpent { get; set; }
        public int UsedRock { get; set; }
        public int UsedPaper { get; set; }
        public int UsedScissors { get; set; }
        public int Points { get; set; }
        
        
    }
}