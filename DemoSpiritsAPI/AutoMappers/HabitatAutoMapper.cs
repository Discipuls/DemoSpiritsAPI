using AutoMapper;
using DemoSpiritsAPI.DTOs.GeoPointDTOs;
using DemoSpiritsAPI.DTOs.HabitatDTOs;
using DemoSpiritsAPI.Models;

namespace DemoSpiritsAPI.AutoMappers
{
    public class HabitatAutoMapper : Profile
    {
        public HabitatAutoMapper() {
            CreateMap<CreateHabitatDTO, Habitat>();
            CreateMap<UpdateHabitatDTO, Habitat>();
            CreateMap<GetHabitatDTO, Habitat>().ReverseMap();
            CreateMap<CreateGeoPointDTO, MarkerPoint>();
            CreateMap<CreateGeoPointDTO, BorderPoint>();
            CreateMap<GetGeoPointDTO, GeoPoint>().ReverseMap();
        }
    }
}
