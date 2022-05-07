using System.Threading.Tasks;
using OneOf;
using Server.Bll.Exceptions;
using Server.Bll.Models;

namespace Server.Bll.Services.Interfaces;

public interface IStatisticsService
{
    Task<ShortStatisticsModel[]> GetAllStatistics();
    Task<OneOf<StatisticsModel, CustomException>> GetPersonalStatistics(string userId);
}