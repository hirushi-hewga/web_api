using Microsoft.AspNetCore.Mvc;
using web_api.DAL;

namespace web_api.Controllers
{
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            return Ok();
        }
    }
}