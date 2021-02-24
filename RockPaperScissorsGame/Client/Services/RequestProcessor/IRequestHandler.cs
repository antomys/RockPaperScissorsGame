using Client.Services.RequestModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Services.RequestProcessor
{
    public interface IRequestHandler
    {
        Task<IResponse> HandleRequestAsync(IRequestOptions requestOptions);
    }
}
