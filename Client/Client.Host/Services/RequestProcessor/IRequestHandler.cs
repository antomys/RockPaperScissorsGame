using System.Threading.Tasks;
using Client.Host.Services.RequestProcessor.RequestModels;

namespace Client.Host.Services.RequestProcessor;

public interface IRequestHandler
{
    Task<IResponse> HandleRequestAsync(IRequestOptions requestOptions);
}