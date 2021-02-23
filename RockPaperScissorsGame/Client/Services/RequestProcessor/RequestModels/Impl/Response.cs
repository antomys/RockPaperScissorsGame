using Client.Services.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.RequestProcessor.RequestModels.Impl
{
    public class Response : IResponse
    {
        public Response(bool handled, int code, string content)
        {
            Handled = handled;
            Code = code;
            Content = content;
        }
        public bool Handled { get; set; }

        public int Code { get; set; }

        public string Content { get; set; }
    }
}
