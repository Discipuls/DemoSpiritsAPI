using DemoSpiritsAPI.DTOs.GeoPointDTOs;
using DemoSpiritsAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DemoSpiritsAPI.DTOs.HabitatDTOs
{
    public class GetHabitatDTO
    {
        public int Id { get; set; }
        public GetGeoPointDTO MarkerLocation {  get; set; }
        public List<GetGeoPointDTO> Border {  get; set; }
        public List<int> SpiritIds { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
