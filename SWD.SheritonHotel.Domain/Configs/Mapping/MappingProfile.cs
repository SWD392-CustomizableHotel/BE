using AutoMapper;
using Entities;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Configs.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UpdateUserCommand>();
            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<ApplicationUser, UpdateUserCommand>()
            .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob.HasValue ? DateOnly.FromDateTime(src.Dob.Value) : (DateOnly?)null));

            CreateMap<UpdateUserCommand, ApplicationUser>()
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob.HasValue ? src.Dob.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null));
        }
    }
}
