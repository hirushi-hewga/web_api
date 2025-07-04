using Microsoft.AspNetCore.Mvc;
using web_api.DAL;
using web_api.DAL.Entities;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/car")]
    public class CarController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult GetCars()
        {
            var cars = _context.Cars.ToList();
            return Ok(cars);
        }

        [HttpPost]
        public IActionResult CreateCar([FromBody] Car entity)
        {
            _context.Cars.Add(entity);
            _context.SaveChanges();
            return Ok($"Автомобіль {entity.Brand} додано");
        }
    }
}