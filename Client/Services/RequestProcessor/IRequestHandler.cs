using System.Threading.Tasks;
using Client.Services.RequestProcessor.RequestModels;

namespace Client.Services.RequestProcessor;

public interface IRequestHandler
{
    Task<IResponse> HandleRequestAsync(IRequestOptions requestOptions);
}