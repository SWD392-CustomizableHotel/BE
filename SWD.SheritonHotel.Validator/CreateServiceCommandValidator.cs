using FluentValidation;
using SWD.SheritonHotel.Domain.Commands.ServiceCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Validator
{
    public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
    {
        public CreateServiceCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .Must(BeAValidDate).WithMessage("StartDate is required and must be a valid date.");
            RuleFor(x => x.EndDate)
                .NotEmpty()
                .Must(BeAValidDate).WithMessage("EndDate is required and must be a valid date.");
            RuleFor(x => x)
                .Must(x => x.EndDate > x.StartDate)
                .WithMessage("EndDate must be greater than StartDate.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
