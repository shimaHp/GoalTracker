

using System.ComponentModel.DataAnnotations;
namespace GoalTracker.UI.Blazor.Validators
{
    

    public class FutureDateIfPresentAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is DateTime date)
            {
                if (date.Date < DateTime.Today)
                {
                    return new ValidationResult(ErrorMessage ?? "Date cannot be in the past.");
                }
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid date value.");
        }
    }
}