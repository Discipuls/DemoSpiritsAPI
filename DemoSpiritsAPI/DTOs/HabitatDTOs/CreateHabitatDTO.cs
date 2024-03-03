using DemoSpiritsAPI.DTOs.GeoPointDTOs;
using DemoSpiritsAPI.Models;

namespace DemoSpiritsAPI.DTOs.HabitatDTOs
{
    public class CreateHabitatDTO
    {
        public CreateGeoPointDTO? MarkerLocation { get; set; }
        public List<CreateGeoPointDTO>? Border {  get; set; }
        public string? Name {  get; set; }
    }
}
