using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiStudy.Models.Repository;

namespace WebApiStudy.Filters
{
    public class Shirt_ValidateShirtFiliterAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var Shirtid = context.ActionArguments["id"] as int?;
            if(Shirtid.HasValue)
            {
                if (Shirtid.Value <= 0)
                {
                    context.ModelState.AddModelError("Shirt", "ShirtID has error");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                else if (!ShirtRepository.ShirtExists(Shirtid.Value))
                {
                    context.ModelState.AddModelError("Shirt", "ShirtID don't exist");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
            }
        }
    }
}
