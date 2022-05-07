using System;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Server.Bll.Exceptions;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;
using Server.Dal.Context;
using OneOf;

namespace Server.Bll.Services;

internal sealed class StatisticsService : IStatisticsService
{
    private readonly ServerContext _repository;

    public StatisticsService(ServerContext repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<ShortStatisticsModel[]> GetAllStatistics()
    {
        return _repository
            .StatisticsEnumerable
            .ProjectToType<ShortStatisticsModel>()
            .ToArrayAsync();
    }

    public async Task<OneOf<StatisticsModel, CustomException>> GetPersonalStatistics(string userId)
    {
        var statistics = await _repository.StatisticsEnumerable
            .Include(stats => stats.Account)
            .FirstOrDefaultAsync(statistics => statistics.Id.Equals(userId));

        return statistics is not null
            ? statistics.Adapt<StatisticsModel>()
            : new CustomException($"Unable to get statistics for user \"{userId}\"");
    }
}