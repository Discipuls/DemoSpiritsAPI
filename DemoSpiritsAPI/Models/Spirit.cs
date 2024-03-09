using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoSpiritsAPI.Models
{
    public enum SpiritType { Forest, Water, Home, Dark, Field }

    public class Spirit : ILastUpdate
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<SpiritType>? Classification { get; set; } = [];
        public List<Habitat>? Habitats { get; set; } = [];

        public MarkerPoint? MarkerLocation { get; set; }


        [JsonIgnore]
        public string? CardImageName { get; set; }
        [JsonIgnore]
        public string? MarkerImageName { get; set; }
        [JsonIgnore]
        public DateTime? LastUpdated { get; set; }

        [NotMapped]
        public Byte[] CardImage { get; set; } = [];
        [NotMapped]
        public Byte[] MarkerImage { get; set; } = [];
    }
}
