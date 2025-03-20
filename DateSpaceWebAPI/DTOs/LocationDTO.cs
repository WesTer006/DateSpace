using NetTopologySuite.Geometries;

namespace DateSpaceWebAPI.DTOs
{
    public class LocationDTO
    {
        public Point? GeoLocation { get; set; }
    }
}
