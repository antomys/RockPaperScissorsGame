using OneOf;

namespace RockPaperScissors.Common.Client;

public interface IClient
{
    Task<OneOf<T, CustomException>> GetAsync<T>(
        string url,
        CancellationToken cancellationToken = default);
    
    Task<OneOf<T, CustomException>> GetAsync<T>(
        string url,
        (string HeaderKey, string HeaderValue) headerValues,
        CancellationToken cancellationToken = default);

    Task<OneOf<T, CustomException>> PostAsync<T, T1>(
        string url,
        T1 content,
        CancellationToken cancellationToken = default);

    Task<OneOf<T, CustomException>> PostAsync<T, T1>(
        string url, T1 content,
        (string HeaderKey, string HeaderValue) headerValues,
        CancellationToken cancellationToken = default);
}