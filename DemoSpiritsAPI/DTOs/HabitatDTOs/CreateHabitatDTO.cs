using DemoSpiritsAPI.DTOs.GeoPointDTOs;
using DemoSpiritsAPI.Models;

namespace DemoSpiritsAPI.DTOs.HabitatDTOs
{
    public class CreateHabitatDTO
    {
        public List<CreateGeoPointDTO>? Border {  get; set; }
        public string? Name {  get; set; }
    }
}
