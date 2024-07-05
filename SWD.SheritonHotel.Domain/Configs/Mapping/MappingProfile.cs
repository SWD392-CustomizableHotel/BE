using AutoMapper;
using Entities;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWD.SheritonHotel.Domain.Entities;

namespace SWD.SheritonHotel.Domain.Configs.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UpdateUserCommand>();
            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<Room, RoomDetailsDTO>().ReverseMap();
            CreateMap<ApplicationUser, AccountDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());;
            CreateMap<Service, AccountServiceDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AssignedServices.FirstOrDefault().User.UserName));
            CreateMap<AssignServiceDto, AssignedService>();
        }
    }
}
