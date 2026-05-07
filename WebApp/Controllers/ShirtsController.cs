using Microsoft.AspNetCore.Components.Web;
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
            if (TempData["Error"] is string errorsJson)
            {
                var errors = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<string>>>(errorsJson);
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Key, string.Join(", ", error.Value));
                }
            }
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
                try
                {
                    var response = await webApiExecuter.InvokePost("shirts", shirt);
                    if (response != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch(WebApiException ex)
                {
                    HandleWebApiException(ex);
                }
            }
            return View(shirt);
        }
        public async Task<IActionResult> UpdateShirt(int shirtId)
        {
            try
            {
                var shirt = await webApiExecuter.InvokeGet<Shirt>($"shirts/{shirtId}");
                if (shirt != null)
                {
                    return View(shirt);
                }
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateShirt(int shirtId, Shirt shirt)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await webApiExecuter.InvokePut<Shirt>($"shirts/{shirtId}", shirt);
                    return RedirectToAction(nameof(Index));
                }
                catch (WebApiException ex)
                {
                    HandleWebApiException(ex);
                }
            }
            return View(shirt);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteShirt([FromForm] int shirtId)
        {
            try
            {
                await webApiExecuter.InvokeDelete<Shirt>($"shirts/{shirtId}");
                return RedirectToAction(nameof(Index));
            }
            catch(WebApiException ex)
            {
                HandleWebApiException(ex);
                return RedirectToAction(nameof(Index));
            }
        }
        private void HandleWebApiException(WebApiException ex)
        {
            if (ex.ErrorResponse != null && ex.ErrorResponse.Errors != null && ex.ErrorResponse.Errors.Count > 0)
            {
                foreach (var error in ex.ErrorResponse.Errors)
                {
                    ModelState.AddModelError(error.Key, string.Join(";  ", error.Value));
                }
                TempData["Error"] = System.Text.Json.JsonSerializer.Serialize(ex.ErrorResponse.Errors);
            }
        }
    }
}
