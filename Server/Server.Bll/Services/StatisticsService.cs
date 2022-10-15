using System;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Common;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;
using OneOf;
using Server.Bll.Extensions;
using Server.Data.Context;
using Server.Data.Entities;

namespace Server.Bll.Services;

internal sealed class StatisticsService : IStatisticsService
{
    private readonly ServerContext _repository;

    public StatisticsService(ServerContext repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<ShortStatisticsModel[]> GetAllAsync()
    {
        return _repository
            .StatisticsEnumerable
            .Include(statistics => statistics.Account)
            .OrderByDescending(statistics => statistics.Score)
            .Take(10)
            .ProjectToType<ShortStatisticsModel>()
            .ToArrayAsync();
    }

    public async Task<OneOf<StatisticsModel, CustomException>> GetAsync(string userId)
    {
        var statistics = await _repository.StatisticsEnumerable
            .Include(stats => stats.Account)
            .FirstOrDefaultAsync(statistics => statistics.Id.Equals(userId));

        if (statistics is null)
        {
            return new CustomException($"Unable to get statistics for user \"{userId}\"");
        }

        return statistics.Adapt<StatisticsModel>();
    }
}