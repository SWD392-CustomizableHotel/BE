using AutoMapper;
using Entities;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
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
        }
    }
}