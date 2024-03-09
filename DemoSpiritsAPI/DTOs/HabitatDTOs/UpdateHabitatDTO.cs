using DemoSpiritsAPI.DTOs.GeoPointDTOs;
using DemoSpiritsAPI.Models;

namespace DemoSpiritsAPI.DTOs.HabitatDTOs
{
    public class UpdateHabitatDTO
    {
        public int Id { get; set; }
        public List<CreateGeoPointDTO> Border {  get; set; }
        public string Name {  get; set; }
    }
}
