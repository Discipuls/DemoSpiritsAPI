using DemoSpiritsAPI.DTOs.GeoPointDTOs;
using DemoSpiritsAPI.Models;

namespace DemoSpiritsAPI.DTOs.SpiritDTOs
{
    public class GetSpiritBasicsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SpiritType> Classification { get; set; }
        public List<int> HabitatsIds { get; set; }
        public DateTime LastUpdated { get; set; }
        public GetGeoPointDTO MarkerLocation { get; set; }

    }
}
