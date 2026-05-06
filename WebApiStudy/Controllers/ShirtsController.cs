using Microsoft.AspNetCore.Mvc;
using WebApiStudy.Data;
using WebApiStudy.Filters.ActionFiliters;
using WebApiStudy.Filters.ExceptinFiliters;
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
        [TypeFilter(typeof(Shirt_ValidateShirtIdFiliterAttribute))]
        public IActionResult GetShirtById(int id)
        {
            return Ok(db.Shirts.Find(id));
        }
        [HttpPost]
        [TypeFilter(typeof(Shirt_ValidateCreateShirtFiliterAttribute))]
        public IActionResult CreateShirt([FromBody]Shirt shirt)
        {

            db.Shirts.Add(shirt);
            db.SaveChanges();
            return CreatedAtAction(nameof(GetShirtById),new { id = shirt.ShirtId },shirt);
        }
        [HttpPut("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFiliterAttribute))]
        [TypeFilter(typeof(Shirt_VaildateUpdateShirtFiliterAttribute))]
        [TypeFilter(typeof(Shirt_HandleUpdateExceptionsFilierAttribute))]
        public IActionResult UpdateShirt(int id, Shirt shirt)
        {
            shirt.ShirtId = id;
            db.Shirts.Update(shirt);
            db.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFiliterAttribute))]
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
