using OneOf;
using RockPaperScissors.Common;
using RockPaperScissors.Common.Responses;

namespace Client.Statistics.Services;

public interface IStatisticsService
{
    Task<OneOf<AllStatisticsResponse[], CustomException>> GetAllAsync(CancellationToken cancellationToken);

    Task<OneOf<PersonalStatisticsResponse, CustomException>> GetPersonalAsync(
        string? token,
        CancellationToken cancellationToken);
}