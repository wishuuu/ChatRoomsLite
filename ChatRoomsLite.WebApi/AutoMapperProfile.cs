using AutoMapper;
using ChatRoomsLite.Models.Entities.User;
using ChatRoomsLite.Models.Entities.User.DTOs;

namespace ChatRoomsLite.WebApi;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<User, UserRegisterDto>();
        CreateMap<UserRegisterDto, User>();
    }
}