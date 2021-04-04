using Server.Contracts;
using Server.Models;

namespace Server.Mappings
{
    public static class StatisticsMappings
    {
        /// <summary>
        /// Method to map Big statistics to a small overall statistics
        /// </summary>
        /// <param name="statistics">Big statistics of all accounts</param>
        /// <returns>StatisticsDto</returns>
        public static StatisticsDto ToStatisticsDto(this Statistics statistics)
        {
            return statistics == null
                ? null
                : new StatisticsDto
                {
                    Login = statistics.Login,
                    Score = statistics.Score
                    
                };
            
        }
    }
}