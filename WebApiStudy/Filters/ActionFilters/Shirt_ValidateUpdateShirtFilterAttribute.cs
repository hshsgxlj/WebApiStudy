using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiStudy.Data;
using WebApiStudy.Models;

namespace WebApiStudy.Filters.ActionFilters
{
    public class Shirt_ValidateUpdateShirtFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;
        public Shirt_ValidateUpdateShirtFilterAttribute(ApplicationDbContext db)
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
                context.ModelState.AddModelError("shirt", "Shirt don't exist");
                var problemDetail = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetail);
            }
        }
    }
}
