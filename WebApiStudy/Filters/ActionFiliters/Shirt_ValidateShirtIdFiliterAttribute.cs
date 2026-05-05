using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiStudy.Data;

namespace WebApiStudy.Filters.ActionFiliters
{
    public class Shirt_ValidateShirtIdFiliterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public Shirt_ValidateShirtIdFiliterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var shirtId = context.ActionArguments["id"] as int?;
            if (shirtId.HasValue)
            {
                if (shirtId.Value <= 0)
                {
                    context.ModelState.AddModelError("Shirt", "ShirtID has error");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                else
                {
                    var existingShirt = db.Shirts.Find(shirtId.Value);
                    if (existingShirt == null)
                    {
                        context.ModelState.AddModelError("Shirt", "ShirtID don't exist");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["shirt"] = existingShirt;
                    }
                }
            }
        }
    }
}
