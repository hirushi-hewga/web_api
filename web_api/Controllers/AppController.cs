using Microsoft.AspNetCore.Mvc;

namespace web_api.Controllers
{
    public class AppController : ControllerBase
    {
        protected bool ValidateId(string? id, out string message)
        {
            if (string.IsNullOrEmpty(id))
            {
                message = "Id is empty";
                return false;
            }
            
            if (!Guid.TryParse(id, out var value))
            {
                message = "Incorrect id format";
                return false;
            }
        
            message = "Id correct";
            return true;
        }
    }
}