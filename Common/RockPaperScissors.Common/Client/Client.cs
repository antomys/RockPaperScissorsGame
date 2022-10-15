using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using OneOf;

namespace RockPaperScissors.Common.Client;

public sealed class Client : IClient
{
    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = null,
        DefaultRequestVersion = HttpVersion.Version20,
        DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
        DefaultRequestHeaders =
        {
            {"Accept", MediaTypeNames.Application.Json}
        }
    };

    private Client(string baseAddress)
    {
        HttpClient.BaseAddress = new Uri(baseAddress);
    }

    public static Client Create(string baseAddress) => new(baseAddress);

    public async Task<OneOf<T, CustomException>> GetAsync<T>(
        string url,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(HttpClient.BaseAddress!, url),
            Version = HttpClient.DefaultRequestVersion,
            VersionPolicy = HttpVersionPolicy.RequestVersionOrLower
        };

        using var response =
            await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return (await JsonSerializer.DeserializeAsync<CustomException>(responseStream, cancellationToken: cancellationToken))!;
        }
        
        return (await JsonSerializer.DeserializeAsync<T>(responseStream, cancellationToken: cancellationToken))!;
    }
    
    public async Task<OneOf<T, CustomException>> GetAsync<T>(
        string url,
        (string HeaderKey, string HeaderValue) headerValues,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            Headers = { 
                {
                    headerValues.HeaderKey, headerValues.HeaderValue
                } 
            },
            RequestUri = new Uri(HttpClient.BaseAddress!, url),
            Version = HttpClient.DefaultRequestVersion,
            VersionPolicy = HttpVersionPolicy.RequestVersionOrLower
        };

        using var response =
            await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return (await JsonSerializer.DeserializeAsync<CustomException>(responseStream, cancellationToken: cancellationToken))!;
        }
        
        return (await JsonSerializer.DeserializeAsync<T>(responseStream, cancellationToken: cancellationToken))!;
    }

    public async Task<bool> GetAsync(
        string url,
        CancellationToken cancellationToken)
    {
        try
        {
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(HttpClient.BaseAddress!, url),
                Version = HttpClient.DefaultRequestVersion,
                VersionPolicy = HttpVersionPolicy.RequestVersionOrLower
            };

            using var response =
                await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<OneOf<T, CustomException>> PostAsync<T, T1>(
        string url,
        T1 content,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, MediaTypeNames.Application.Json),
            RequestUri = new Uri(HttpClient.BaseAddress!, url),
            Version = HttpClient.DefaultRequestVersion,
            VersionPolicy = HttpVersionPolicy.RequestVersionOrLower
        };

        using var response =
            await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return (await JsonSerializer.DeserializeAsync<CustomException>(responseStream, cancellationToken: cancellationToken))!;
        }
        
        return (await JsonSerializer.DeserializeAsync<T>(responseStream, cancellationToken: cancellationToken))!;
    }
    
    public async Task<OneOf<T, CustomException>> PostAsync<T, T1>(
        string url,
        T1 content,
        (string HeaderKey, string HeaderValue) headerValues,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Headers = { 
                {
                    headerValues.HeaderKey, headerValues.HeaderValue
                } 
            },
            Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, MediaTypeNames.Application.Json),
            RequestUri = new Uri(HttpClient.BaseAddress!, url),
            Version = HttpClient.DefaultRequestVersion,
            VersionPolicy = HttpVersionPolicy.RequestVersionOrLower
        };

        using var response =
            await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return (await JsonSerializer.DeserializeAsync<CustomException>(responseStream, cancellationToken: cancellationToken))!;
        }
        
        return (await JsonSerializer.DeserializeAsync<T>(responseStream, cancellationToken: cancellationToken))!;
    }
}