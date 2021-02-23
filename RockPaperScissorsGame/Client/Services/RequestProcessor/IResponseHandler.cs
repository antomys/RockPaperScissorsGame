using Client.Services.RequestModels;
using System;
using System.Threading.Tasks;

namespace Client.Services.RequestProcessor
{
    public interface IResponseHandler
    {
        Task HandleResponseAsync(IResponse response, IRequestOptions requestOptions, IResponseOptions responseOptions);
    }
}
