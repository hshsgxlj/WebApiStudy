using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiStudy.Data;

namespace WebApiStudy.Filters.ExceptinFiliters
{
    public class Shirt_HandleUpdateExceptionsFilierAttribute : ExceptionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public Shirt_HandleUpdateExceptionsFilierAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnException(ExceptionContext context)
        {
            var strShirtId = context.RouteData.Values["id"] as string;
            if (int.TryParse(strShirtId, out int shirtId))
            {
                var shirt = db.Shirts.Find(shirtId);
                if (shirt == null)
                {
                    context.ModelState.AddModelError("ShirtId", "ShirtId don't exist");
                    var problemDetail = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetail);
                    context.ExceptionHandled = true;
                }
            }
        }
    }
}
