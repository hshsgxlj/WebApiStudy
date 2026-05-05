using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Validations
{
    public class Shirt_EnsureCorrect: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var shirt = validationContext.ObjectInstance as Shirt;
            if(shirt!=null)
            {
                if(string.Equals(shirt.Gender, "men", StringComparison.OrdinalIgnoreCase) && shirt.Size < 8)
                {
                    return new ValidationResult("For men's Shirt,the size should has to be greater or equal to 8");
                }
                
            }
            return ValidationResult.Success;

        }
    }
}