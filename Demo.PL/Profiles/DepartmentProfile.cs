using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.Profiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentViewModel, Department>();
        }
    }
}
