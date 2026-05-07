using Microsoft.AspNetCore.Mvc;
using WebApiStudy.Data;
using WebApiStudy.Filters.ActionFilters;
using WebApiStudy.Filters.ExceptionFilters;
using WebApiStudy.Models;

namespace WebApiStudy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShirtsController:ControllerBase
    {
        private readonly ApplicationDbContext db;
        public ShirtsController(ApplicationDbContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public IActionResult GetShirts()
        {
            return Ok(db.Shirts.ToList());
        }
        [HttpGet("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        public IActionResult GetShirtById(int id)
        {
            return Ok(db.Shirts.Find(id));
        }
        [HttpPost]
        [TypeFilter(typeof(Shirt_ValidateCreateShirtFilterAttribute))]
        public IActionResult CreateShirt([FromBody]Shirt shirt)
        {

            db.Shirts.Add(shirt);
            db.SaveChanges();
            return CreatedAtAction(nameof(GetShirtById),new { id = shirt.ShirtId },shirt);
        }
        [HttpPut("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [TypeFilter(typeof(Shirt_ValidateUpdateShirtFilterAttribute))]
        [TypeFilter(typeof(Shirt_HandleUpdateExceptionsFilterAttribute))]
        public IActionResult UpdateShirt(int id, Shirt shirt)
        {
            shirt.ShirtId = id;
            db.Shirts.Update(shirt);
            db.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        public IActionResult DeleteShirt(int id)
        {
            var shirt = db.Shirts.Find(id);
            if (shirt != null)
            {
                db.Shirts.Remove(shirt);
                db.SaveChanges();
            }
            return Ok($"Delete shirt:{id}");
        }
        
    }
}
