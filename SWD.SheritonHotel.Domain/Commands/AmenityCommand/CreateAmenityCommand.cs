﻿using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace SWD.SheritonHotel.Domain.Commands.AmenityCommand
{
    public class CreateAmenityCommand : IRequest<ResponseDto<Amenity>>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public int InUse { get; set; }

        [Required]
        public decimal Price { get; set; }

        [DefaultValue("Normal")]
        public AmenityStatus Status { get; set; } = AmenityStatus.Normal;
        [Required]
        public int HotelId { get; set; }
    }
}
