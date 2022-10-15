using OneOf;
using RockPaperScissors.Common;
using RockPaperScissors.Common.Client;
using RockPaperScissors.Common.Responses;

namespace Client.Statistics.Services;

internal sealed class StatisticsService: IStatisticsService
{
    private readonly IClient _client;

    public StatisticsService(IClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public Task<OneOf<AllStatisticsResponse[], CustomException>> GetAllAsync(CancellationToken cancellationToken)
    {
        var response =
            _client.GetAsync<AllStatisticsResponse[]>("api/Statistics/all", cancellationToken);

        return response;
    }
    
    public Task<OneOf<PersonalStatisticsResponse, CustomException>> GetPersonalAsync(string? token, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(token))
        {
            return Task.FromResult(OneOf<PersonalStatisticsResponse, CustomException>.FromT1(new CustomException("Token must not be 'null' or '\"\"'")));
        }
        
        throw new NotImplementedException();
    }
}