using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Bll.Models;

namespace Server.Bll.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<IEnumerable<StatisticsModel>> GetAllStatistics();
        Task<StatisticsModel> GetPersonalStatistics(int userId);
    }
}