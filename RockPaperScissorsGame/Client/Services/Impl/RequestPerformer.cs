using Client.Services.RequestModels;
using Client.Services.RequestProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.Impl
{
    public class RequestPerformer : IRequestPerformer
    {
        public Task<bool> PerformRequestAsync(IRequestOptions requestOptions, IResponseOptions responseOptions)
        {
            throw new NotImplementedException();
        }
    }
}
