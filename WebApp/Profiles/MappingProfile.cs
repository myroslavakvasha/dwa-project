using AutoMapper;
using BL.DTOs.Auth;
using BL.DTOs.Food;
using WebApp.Models.Auth;
using WebApp.Models.Food;

namespace WebApp.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterVM, UserRegisterDto>();
            CreateMap<FoodResponseDto, FoodRowVM>();
        }
    }
}
