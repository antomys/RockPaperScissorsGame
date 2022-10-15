using System.Threading.Tasks;
using Client.Host.Services.RequestProcessor.RequestModels;

namespace Client.Host.Services.RequestProcessor;

public interface IRequestPerformer
{
    Task<IResponse> PerformRequestAsync(IRequestOptions requestOptions);
}