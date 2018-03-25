using FluentValidation;
using Toffees.Glucose.Models.DTOs;

namespace Toffees.Glucose
{
    public class GlucoseValidator : AbstractValidator<GlucoseDto>
    {
        public GlucoseValidator()
        {
            RuleFor(request => request.Data).NotEmpty().WithMessage("Please specify a valid Blood Glucose value.");
            RuleFor(request => request.Data).ExclusiveBetween(40, 500);
            RuleFor(request => request.PinchDateTime).NotEmpty().WithMessage("Please specify a DateTime string in ISO format.");
            RuleFor(request => request.Tag).NotEmpty().WithMessage("Please enter a Blood Glucose Tag.");
            RuleFor(request => request.Tag).Matches(@"^[A-Z]+[a-zA-Z''-'\s]*$").WithMessage("Please specify a valid Blood Glucose Tag.");
            RuleFor(request => request.Tag).Length(3, 30);
        }
    }
}
