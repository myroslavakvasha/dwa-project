using AutoMapper;
using BL.DTOs.Allergen;
using BL.DTOs.Auth;
using BL.DTOs.Category;
using BL.DTOs.Food;
using BL.DTOs.Order;
using WebApp.ViewModels.Allergen;
using WebApp.ViewModels.Auth;
using WebApp.ViewModels.Category;
using WebApp.ViewModels.Food;
using WebApp.ViewModels.Order;
using WebApp.ViewModels.User;

namespace WebApp.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterVM, UserRegisterDto>();

            CreateMap<FoodResponseDto, FoodRowVM>();
            CreateMap<FoodFormVM, FoodRequestDto>();

            CreateMap<CategoryResponseDto, CategoryVM>();
            CreateMap<CategoryVM, CategoryRequestDto>();

            CreateMap<AllergenResponseDto, AllergenVM>();
            CreateMap<AllergenVM, AllergenRequestDto>();

            CreateMap<UserResponseDto, UserRowVM>();

            CreateMap<OrderResponseDto, OrderRowVM>();
            CreateMap<OrderItemResponseDto, OrderItemVM>();
            CreateMap<OrderDetailResponseDto, OrderDetailVM>();

            CreateMap<UserResponseDto, ProfileVM>();
            CreateMap<ProfileVM, UserUpdateDto>();

        }
    }
}
