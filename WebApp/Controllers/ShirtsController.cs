using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;
using WebApp.Models.Repository;

namespace WebApp.Controllers
{
    public class ShirtsController : Controller
    {
        private readonly IWebApiExecuter webApiExecuter;

        public ShirtsController(IWebApiExecuter webApiExecuter)
        {
            this.webApiExecuter = webApiExecuter;
        }

        public async Task<IActionResult> Index()
        {
            return View(await webApiExecuter.InvokeGet<List<Shirt>>("shirts"));
        }
        public IActionResult CreateShirt()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateShirt(Shirt shirt)
        {
            if (ModelState.IsValid)
            {
                var resronse = await webApiExecuter.InVokePost("shirts", shirt);
                if (resronse != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(shirt);
        }
        public async Task<IActionResult> UpdateShirt(int shirtId)
        {
            var shirt = await webApiExecuter.InvokeGet<Shirt>($"shirts/{shirtId}");
            if (shirt != null)
            {
                return View(shirt);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateShirt(int shirtId, Shirt shirt)
        {
            if (ModelState.IsValid)
            {
                await webApiExecuter.InvokePut<Shirt>($"shirts/{shirtId}", shirt);
                return RedirectToAction(nameof(Index));
            }
            return View(shirt);
        }
    }
}
