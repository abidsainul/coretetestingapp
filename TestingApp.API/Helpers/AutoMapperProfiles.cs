using System.Linq;
using AutoMapper;
using TestingApp.API.Dtos;
using TestingApp.API.Models;

namespace TestingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User,UserForListDto>()
                .ForMember(dest => dest.PhotoUrl , opt => 
                    opt.MapFrom(src => src.PlantPhotos.FirstOrDefault(p =>p.IsMain).url))
                .ForMember(dest => dest.Age, opt =>
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<User,UserForDetailedDto>()
                 .ForMember(dest => dest.PhotoUrl , opt => 
                    opt.MapFrom(src => src.PlantPhotos.FirstOrDefault(p =>p.IsMain).url))
                .ForMember(dest => dest.Age, opt =>
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            
            CreateMap<UserForUpdateDto , User>();
            
            CreateMap<PlantPhoto,PlantPhotoForDetailedDto>();
            CreateMap<PlantPhoto,PlantPhotoForReturnDto>();
            CreateMap<PlantPhotoForCreationDto , PlantPhoto> ();
        }
    }
}