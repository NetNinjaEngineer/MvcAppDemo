using AutoMapper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleViewModel>()
                .ForMember(x => x.RoleName, opt => opt.MapFrom(x => x.Name))
                .ForMember(x => x.RoleId, opt => opt.MapFrom(x => x.Id))
                .ReverseMap();
        }
    }
}
