using NetTopologySuite.Geometries;

namespace BusinessLayer.Models
{
    public class LocationModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public Point? GeoLocation { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
