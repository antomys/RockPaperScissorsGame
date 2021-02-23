using Client.Services.RequestModels;
using System;
using System.Threading.Tasks;

namespace Client.Services.RequestProcessor
{
    public interface IRequestPerformer
    {
        Task<bool> PerformRequestAsync(IRequestOptions requestOptions, IResponseOptions responseOptions);
    }
}
