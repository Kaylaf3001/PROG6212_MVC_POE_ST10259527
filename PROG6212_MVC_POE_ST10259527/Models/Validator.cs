using FluentValidation;
using PROG6212_MVC_POE_ST10259527.Models;

namespace PROG6212_MVC_POE_ST10259527.Validators
{
    public class ClaimsModelValidator : AbstractValidator<ClaimsModel>
    {
        public ClaimsModelValidator()
        {
            RuleFor(claim => claim.HoursWorked)
                .GreaterThan(0).WithMessage("Hours worked must be greater than 0.")
                .WithSeverity(Severity.Warning); // Change to warning

            RuleFor(claim => claim.HourlyRate)
                .GreaterThan(0).WithMessage("Hourly rate must be greater than 0.");

            RuleFor(claim => claim.CalculateTotalAmount())
                .Equal(claim => claim.HoursWorked * claim.HourlyRate)
                .WithMessage("Calculated total amount does not match the expected value.");

            RuleFor(claim => claim.SupportingDocumentName)
                .NotEmpty().WithMessage("Supporting document must be provided.");
        }
    }
}
