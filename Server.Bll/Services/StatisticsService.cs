using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;
using Server.Dal.Context;

namespace Server.Bll.Services;

internal sealed class StatisticsService : IStatisticsService
{
    private readonly ServerContext _repository;

    public StatisticsService(ServerContext repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ShortStatisticsModel>> GetAllStatistics()
    {
        return await _repository
            .StatisticsEnumerable
            .ProjectToType<ShortStatisticsModel>()
            .ToArrayAsync();
    }

    public async Task<StatisticsModel> GetPersonalStatistics(int userId)
    {
        var statistics = await _repository.StatisticsEnumerable
            .Include(x=> x.Account)
            .FirstOrDefaultAsync(statistics => statistics.Id == userId);
            
        return statistics.Adapt<StatisticsModel>();
    }
}