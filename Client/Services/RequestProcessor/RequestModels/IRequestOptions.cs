using System.Collections.Generic;
using Client.Services.RequestModels;

namespace Client.Services.RequestProcessor.RequestModels
{
    public interface IRequestOptions
    {
        Dictionary<string, string> Headers { get; }
        string Name { get; }
        string Address { get; }
        RequestMethod Method { get; }
        string ContentType { get; }
        string Body { get; }
        bool IsValid { get; }
    }
}
