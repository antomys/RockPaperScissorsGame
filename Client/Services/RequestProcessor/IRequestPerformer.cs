using Client.Services.RequestModels;
using Client.Services.RequestProcessor.RequestModels.Impl;
using System;
using System.Threading.Tasks;
using Client.Services.RequestProcessor.RequestModels;

namespace Client.Services.RequestProcessor
{
    public interface IRequestPerformer
    {
        Task<IResponse> PerformRequestAsync(IRequestOptions requestOptions);
    }
}
