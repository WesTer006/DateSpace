using System;
using NetTopologySuite.Geometries;

namespace DataAccessLayer.Entities
{
	public class Location
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public Point GeoLocation { get; set; }  // Изменено с DbGeography на Point
		public DateTime UpdatedAt { get; set; }

		public AppUser User { get; set; }
	}
}
