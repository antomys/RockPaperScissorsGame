using Client.Services.RequestProcessor.RequestModels.Impl;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Client.Services.RequestProcessor.RequestModels;

namespace Client.Services.RequestProcessor.Impl;

public class RequestHandler : IRequestHandler
{
    private readonly HttpClient _client;
    private readonly HttpClientHandler _httpClientHandler;

    public RequestHandler(HttpClient httpClient, HttpClientHandler httpClientHandler)
    {
        _client = httpClient;
        _httpClientHandler = httpClientHandler;
    }
    public async Task<IResponse> HandleRequestAsync(IRequestOptions requestOptions)
    {
        //var handler = new HttpClientHandler();
        _httpClientHandler
                .ServerCertificateCustomValidationCallback 
            += (_, _, _, _) => true;
        if (requestOptions == null) throw new ArgumentNullException(nameof(requestOptions));
        if (!requestOptions.IsValid) throw new ArgumentOutOfRangeException(nameof(requestOptions));

        using var msg = 
            new HttpRequestMessage(MapMethod(requestOptions.Method), 
                new Uri(_client.BaseAddress+requestOptions.Address));
        try
        {
            if(requestOptions.Headers != null)
                foreach (var (key, value) in requestOptions.Headers)
                {
                    msg.Headers.Add(key,value);
                }
                
            if (MapMethod(requestOptions.Method) == HttpMethod.Delete)
            {
                using var responseD = await _client.SendAsync(msg);
                var bodyD = await responseD.Content.ReadAsStringAsync();
                return new Response(true, (int)responseD.StatusCode, bodyD);
            }

            if (MapMethod(requestOptions.Method) != HttpMethod.Get)
            {
                msg.Content = new StringContent(requestOptions.Body, Encoding.UTF8, requestOptions.ContentType);
                using var responseForPushingData = await _client.SendAsync(msg);
                var bodyForPushing = await responseForPushingData.Content.ReadAsStringAsync();
                return new Response(true, (int)responseForPushingData.StatusCode, bodyForPushing);
            }
                
            using var response = await _client.SendAsync(msg);
            var body = await response.Content.ReadAsStringAsync();
            return new Response(true, (int)response.StatusCode, body);
        }
        catch (HttpRequestException) //todo: probably redo
        {
            return new Response(false, 500, "Server.Host is not responding!");
        }
    }
    private static HttpMethod MapMethod(RequestMethod method)
    {
        return method switch
        {
            RequestMethod.Get => HttpMethod.Get,
            RequestMethod.Post => HttpMethod.Post,
            RequestMethod.Put => HttpMethod.Put,
            RequestMethod.Patch => HttpMethod.Patch,
            RequestMethod.Delete => HttpMethod.Delete,
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, "Invalid request method")
        };
    }
}