using Microsoft.AspNetCore.Mvc;
using System.Collections;
using WebApiStudy.Filters;
using WebApiStudy.Models;
using WebApiStudy.Models.Repository;

namespace WebApiStudy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShirtsController:ControllerBase
    {
       
        [HttpGet]
        public IActionResult GetShirts()
        {
            return Ok(ShirtRepository.GetShirts());
        }
        [HttpGet("{id}")]
        [Shirt_ValidateShirtFiliter]
        public IActionResult GetShirtById(int id)
        {
            return Ok(ShirtRepository.GetShirtById(id));
        }
        [HttpPost("{id}")]
        public IActionResult CreateShirt([FromForm]Shirt shirt)
        {
            return Ok($"Creating a shirt");
        }
        [HttpPut("{id}")]
        public IActionResult Updateshirt(int id)
        {
            return Ok($"Updating shirt:{id}");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteShirt(int id)
        {
            return Ok($"Delete shirt:{id}");
        }
    }
}
