using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_api.BLL.Services
{
    public class ServiceResponse
    {
        public ServiceResponse() { }
        public ServiceResponse(string message, bool isSuccess = false, object? payload = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Payload = payload;
        }

        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public object? Payload { get; set; }
    }
}
