using Client.Models.Interfaces;

namespace Client.Models
{
    internal class Statistics : IStatistics, IOverallStatistics
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Draws { get; set; }
        public double WinLossRatio { get; set; }
        public string TimeSpent { get; set; }
        public int UsedRock { get; set; }
        public int UsedPaper { get; set; }
        public int UsedScissors { get; set; }

        public int Score { get; set; }

        public override string ToString()
        {
            return $"Login: {Login}; Score: {Score}";
        }
    }
}