using System;
using System.Threading.Tasks;
using Client.Services.RequestProcessor.RequestModels;

namespace Client.Services.RequestProcessor.Impl;

public class RequestPerformer : IRequestPerformer
{
    private readonly IRequestHandler _requestHandler;
    public RequestPerformer(IRequestHandler requestHandler)
    {
        _requestHandler = requestHandler;
    }
    public async Task<IResponse> PerformRequestAsync(IRequestOptions requestOptions)
    { 
        if (requestOptions == null) throw new ArgumentNullException(nameof(requestOptions));
        if (!requestOptions.IsValid) throw new ArgumentOutOfRangeException(nameof(requestOptions)); 
        try
        {
            return await _requestHandler.HandleRequestAsync(requestOptions);
        }
        catch (TimeoutException) //todo: Probably redo
        {
            //response = new Response(false, 408, null);
            return null;
        }
    }
}