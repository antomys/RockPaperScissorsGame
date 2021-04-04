using Client.Services.RequestModels;
using Client.Services.RequestProcessor.RequestModels.Impl;
using System;
using System.Threading.Tasks;

namespace Client.Services.RequestProcessor
{
    public interface IRequestPerformer
    {
        Task<IResponse> PerformRequestAsync(IRequestOptions requestOptions);
    }
}
