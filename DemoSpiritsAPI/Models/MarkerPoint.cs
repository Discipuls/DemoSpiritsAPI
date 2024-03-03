using System.ComponentModel.DataAnnotations.Schema;

namespace DemoSpiritsAPI.Models
{
    public class MarkerPoint : GeoPoint
    {

        public Habitat Habitat { get; set; } = null!;

    }
}
