using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(x => x.FName, opt => opt.MapFrom(u => u.FirstName))
                .ForMember(x => x.LName, opt => opt.MapFrom(u => u.LastName))
                .ReverseMap();
        }
    }
}
