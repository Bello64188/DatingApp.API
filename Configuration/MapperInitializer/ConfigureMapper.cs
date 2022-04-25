using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Models;

namespace DatingApp.API.Configuration.MapperInitializer
{
    public class ConfigureMapper : Profile
    {
        public ConfigureMapper()
        {
            CreateMap<UserApi, UserDTO>().ReverseMap();
        }
    }
}