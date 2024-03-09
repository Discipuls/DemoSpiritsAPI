using System.ComponentModel.DataAnnotations.Schema;

namespace DemoSpiritsAPI.Models
{
    public class MarkerPoint : GeoPoint
    {

        public Spirit spirit{ get; set; } = null!;

    }
}
