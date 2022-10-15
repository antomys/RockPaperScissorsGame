using System.Threading.Tasks;
using OneOf;
using RockPaperScissors.Common;
using Server.Bll.Models;

namespace Server.Bll.Services.Interfaces;

public interface IStatisticsService
{
    Task<ShortStatisticsModel[]> GetAllAsync();
    
    Task<OneOf<StatisticsModel, CustomException>> GetAsync(string userId);
}