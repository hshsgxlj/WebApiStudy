using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiStudy.Data;
using WebApiStudy.Models;

namespace WebApiStudy.Filters.ActionFiliters
{
    public class Shirt_VaildateUpdateShirtFiliterAttribute:ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;
        public Shirt_VaildateUpdateShirtFiliterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var shirtId = context.ActionArguments["id"] as int?;
            var shirt = context.ActionArguments["shirt"] as Shirt;

            if (!shirtId.HasValue || shirt == null)
            {
                return;
            }

            if (shirtId != shirt.ShirtId)
            {
                context.ModelState.AddModelError("shirt", "Id is wrong");
                var problemDetail = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetail);
                return;
            }

            var existingShirt = db.Shirts.Find(shirtId.Value);
            if (existingShirt == null)
            {
                context.ModelState.AddModelError("shirt", "Shirt dont exist");
                var problemDetail = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetail);
            }
        }
    }
}
