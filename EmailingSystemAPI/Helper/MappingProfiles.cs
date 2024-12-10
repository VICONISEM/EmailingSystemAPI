using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs;

namespace EmailingSystemAPI.Helper
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            CreateMap<ApplicationUser, RegisterDto>().ReverseMap();
            CreateMap<ApplicationUser, UserDto>().ForMember(U => U.DepartmentName, O => O.MapFrom(U => U.Department.Name)).ReverseMap();
        }


    }
}
