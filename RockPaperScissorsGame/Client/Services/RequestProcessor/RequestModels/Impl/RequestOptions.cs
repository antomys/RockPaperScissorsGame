using Client.Services.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.RequestProcessor.RequestModels.Impl
{
    public class RequestOptions : IRequestOptions
    {
        public RequestOptions()
        {
            Address = "http://localhost:5000/";
        }
        public string Name { get; set; }

        public string Address { get; set; }

        public RequestMethod Method { get; set; }

        public string ContentType { get; set; }

        public string Body { get; set; }

        public bool IsValid { get; set; }
    }
}
