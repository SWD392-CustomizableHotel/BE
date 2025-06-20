﻿using AutoMapper;
using SWD.SheritonHotel.Domain.Commands.Auth;
using SWD.SheritonHotel.Domain.Commands.PaymentCommand;
using SWD.SheritonHotel.Domain.DTO.Account;
using SWD.SheritonHotel.Domain.DTO.Amenity;
using SWD.SheritonHotel.Domain.DTO.Booking;
using SWD.SheritonHotel.Domain.DTO.IdentityCard;
using SWD.SheritonHotel.Domain.DTO.Payment;
using SWD.SheritonHotel.Domain.DTO.Room;
using SWD.SheritonHotel.Domain.DTO.Service;
using SWD.SheritonHotel.Domain.Entities;

namespace SWD.SheritonHotel.Domain.Configs.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UpdateUserCommand>();
            CreateMap<Payment, PaymentDto>();

            CreateMap<UpdateUserCommand, ApplicationUser>()
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob));

            CreateMap<Room, RoomDto>().ReverseMap();

            CreateMap<IdentityCardDto, IdentityCard>();
            CreateMap<IdentityCard, IdentityCardDto>();

            CreateMap<Room, RoomDetailsDTO>().ReverseMap();
            CreateMap<ApplicationUser, StaffDTO>();
            CreateMap<Service, ServiceDto>()
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                    .ForMember(dest => dest.AssignedStaff, opt => opt.MapFrom(src => src.AssignedStaff));

            CreateMap<ApplicationUser, StaffDto>();
            CreateMap<ApplicationUser, CustomerDTO>();
            CreateMap<ApplicationUser, AccountDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());

            CreateMap<Service, AccountServiceDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AssignedServices.FirstOrDefault().User.UserName));

            CreateMap<AssignServiceDto, AssignedService>();

            CreateMap<CreatePaymentIntentCustomizableCommand, CreatePaymentIntentDTO>().ReverseMap();
            CreateMap<CreatePaymentIntentCustomizableCommand.Item, CreatePaymentIntentDTO.Item>().ReverseMap();
            CreateMap<Booking, BookingDetailsDto>()
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.Room.Type))
                .ForMember(dest => dest.RoomDescription, opt => opt.MapFrom(src => src.Room.Description))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.BookingAmenities.Select(ba => new AmenityDTO
                {
                    Name = ba.Amenity.Name,
                    Code = ba.Amenity.Code,
                    Description = ba.Amenity.Description,
                    Price = ba.Amenity.Price,
                })))
                .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments.Select(p => new PaymentDto
                {
                    Amount = p.Amount,
                    Status = p.Status,
                    PaymentMethod = p.PaymentMethod,
                })))
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());
        }
    }
}