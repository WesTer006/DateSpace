using System;
using NetTopologySuite.Geometries;

namespace DataAccessLayer.Entities
{
	public class Location
	{
		public int Id { get; set; }
		public required string UserId { get; set; }
		public required Point GeoLocation { get; set; }
		public DateTime UpdatedAt { get; set; }

		public required AppUser User { get; set; }
	}
}
