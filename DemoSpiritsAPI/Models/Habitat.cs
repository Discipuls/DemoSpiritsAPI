
namespace DemoSpiritsAPI.Models
{
    public class Habitat  : ILastUpdate
    {
        public int Id {  get; set; }
        public string? Name { get; set; }
        public List<Spirit>? Spirits { get; set; } = [];
        public List<BorderPoint>? Border { get; set; } = [];
        public DateTime? LastUpdated { get; set; }

    }
}
