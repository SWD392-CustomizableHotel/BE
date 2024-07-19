using FluentValidation;
using SWD.SheritonHotel.Domain.Commands.IdentityCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Validator
{
    public class UploadIdentityCardCommandValidator : AbstractValidator<UploadIdentityCardCommand>
    {
        public UploadIdentityCardCommandValidator()
        {
            RuleFor(x => x.FrontFile).NotNull().WithMessage("Front side must be provided.");
            RuleFor(x => x.PaymentId).NotEmpty().WithMessage("Payment ID must be provided.");
        }
    }
}
