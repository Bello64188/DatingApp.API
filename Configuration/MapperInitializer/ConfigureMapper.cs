using System.ComponentModel;
using System.Linq;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Models;

namespace DatingApp.API.Configuration.MapperInitializer
{
    public class ConfigureMapper : Profile
    {
        public ConfigureMapper()
        {
            CreateMap<UserData, UserDTO>().ReverseMap();
            CreateMap<UserData, UserListsDto>().
            ForMember(dest=>dest.photoUrl,opt=>{
                opt.MapFrom(scr=>scr.photos.FirstOrDefault(p=>p.isMain).url);
                }).
                ForMember(dest=>dest.age,opt=>{
                    opt.MapFrom(d=>d.dateOfBirth.CalculateAge());
                });
            
            CreateMap<UserData, UsersDetailsDTO>().
            ForMember(dest=>dest.photoUrl,opt=>{
                opt.MapFrom(p=>p.photos.FirstOrDefault(i=>i.isMain).url);
            })
            .ForMember(dest=>dest.age, opt=>{
                opt.MapFrom(d=>d.dateOfBirth.CalculateAge());
            });

            CreateMap<Photo, PhotoDto>();
        }
    }
}