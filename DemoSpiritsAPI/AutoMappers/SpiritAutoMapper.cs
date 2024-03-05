using AutoMapper;
using DemoSpiritsAPI.DTOs.SpiritDTOs;
using DemoSpiritsAPI.Models;

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
