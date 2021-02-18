using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloveceApiServer.Models
{
    public class HttpResponseData
    {
        public object Data { get; set; }
        public string Message { get; set; }

        public HttpResponseData(string message, object data)
        {
            Message = message;
            Data = data;
        }

        public HttpResponseData(string message)
        {
            Message = message;
            Data = null;
        }
    }
}
