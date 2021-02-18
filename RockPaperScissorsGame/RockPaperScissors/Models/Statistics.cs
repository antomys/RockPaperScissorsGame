using System;
using RockPaperScissors.Models.Interfaces;

namespace RockPaperScissors.Models
{
    internal class Statistics : IStatistics, IPersonalStatistics
    {
        public Statistics(IAccount account)
        {
            Account = account;
        }
        public IAccount Account { get; }
        
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