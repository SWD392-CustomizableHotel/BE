using FluentValidation;
using Microsoft.AspNetCore.Http;
using SWD.SheritonHotel.Domain.Commands.RoomCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Validator
{
    public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomCommandValidator() 
        {
            RuleFor(x => x.Type)
           .NotEmpty().WithMessage("Room type is required.")
           .MaximumLength(100).WithMessage("Room type cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => status == "Available" || status == "Occupied" || status == "Maintenance")
                .WithMessage("Status must be either 'Available' or 'Occupied' or 'Maintenance'.");

            RuleFor(x => x.RoomSize)
                .NotEmpty().WithMessage("RoomSize is required.")
                .MaximumLength(50).WithMessage("Test Room Size");

            RuleFor(x => x.HotelId)
                .GreaterThan(0).WithMessage("Hotel ID must be greater than zero.");

            RuleFor(x => x.ImageUpload)
                .NotNull().WithMessage("Image is required.")
                .Must(IsValidImageFile).WithMessage("Invalid image format.");
        }

        private bool IsValidImageFile(IFormFile file)
        {
            if (file == null)
            {
                return false;
            }

            var validImageTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            return validImageTypes.Contains(file.ContentType);
        }
    }
}
