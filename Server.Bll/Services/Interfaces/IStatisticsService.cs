using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Bll.Models;

namespace Server.Bll.Services.Interfaces;

public interface IStatisticsService
{
    Task<IEnumerable<ShortStatisticsModel>> GetAllStatistics();
    Task<StatisticsModel> GetPersonalStatistics(int userId);
}