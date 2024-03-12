using AutoMapper;
using SpiritsClassLibrary.DTOs.SpiritDTOs;
using SpiritsClassLibrary.Models;

namespace DemoSpiritsAPI.AutoMappers
{
    public class SpiritAutoMapper : Profile
    {
        public SpiritAutoMapper() {
            CreateMap<CreateSpiritDTO, Spirit>();
            CreateMap<UpdateSpiritDTO, Spirit>();
            CreateMap<GetSpiritDTO, Spirit>().ReverseMap();
            CreateMap<GetSpiritBasicsDTO, Spirit>().ReverseMap();
        }
    }
}
