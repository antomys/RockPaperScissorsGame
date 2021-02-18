namespace RockPaperScissors.Models.Interfaces
{
    internal interface IStatistics
    {
        IAccount Account { get; }
        int Points { get; set; }
    }
}