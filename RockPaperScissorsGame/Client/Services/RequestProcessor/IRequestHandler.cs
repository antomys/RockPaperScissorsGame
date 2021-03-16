using Client.Services.RequestModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Client.Services.RequestProcessor.RequestModels;

namespace Client.Services.RequestProcessor
{
    public interface IRequestHandler
    {
        Task<IResponse> HandleRequestAsync(IRequestOptions requestOptions);
    }
}
