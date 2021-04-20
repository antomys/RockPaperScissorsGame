using Client.Services.RequestModels;
using Client.Services.RequestProcessor.RequestModels.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.RequestProcessor.Impl
{
    public class RequestPerformer : IRequestPerformer
    {
        public RequestPerformer()
        {
            RequestHandler = new RequestHandler();
        }
        public readonly IRequestHandler RequestHandler;
        public async Task<IResponse> PerformRequestAsync(IRequestOptions requestOptions)
        {
            IResponse response = null;
            if (requestOptions == null) throw new ArgumentNullException(nameof(requestOptions));
            if (!requestOptions.IsValid) throw new ArgumentOutOfRangeException(nameof(requestOptions));
                try
                {
                    response = await RequestHandler.HandleRequestAsync(requestOptions);
                }
                catch (TimeoutException) //todo: Probably redo
                {
                    response = new Response(false, 408, null);
                }        
            return response;
        }
    }
}
