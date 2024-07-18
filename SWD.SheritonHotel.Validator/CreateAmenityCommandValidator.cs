using FluentValidation;
using SWD.SheritonHotel.Domain.Commands.AmenityCommand;

namespace SWD.SheritonHotel.Validator
{
    public class CreateAmenityCommandValidator : AbstractValidator<CreateAmenityCommand>
    {
        public CreateAmenityCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("Capacity must be greater than zero.");
            RuleFor(x => x.InUse).GreaterThanOrEqualTo(0).WithMessage("InUse must be a non-negative number.");
        }
        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
