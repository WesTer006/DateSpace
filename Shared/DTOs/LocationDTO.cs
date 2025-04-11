using NetTopologySuite.Geometries;

namespace DateSpaceWebAPI.DTOs
{
    public class LocationDto
    {
        public Point? GeoLocation { get; set; }
    }
}
