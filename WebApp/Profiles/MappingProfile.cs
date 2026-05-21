using AutoMapper;
using DAL.DTOs.Auth;
using WebApp.Models;

namespace WebApp.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterVM, UserRegisterDto>();
        }
    }
}
