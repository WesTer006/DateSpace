using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace DataAccessLayer.Repositories
{
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly AppDbContext _context;

        public RecommendationRepository(AppDbContext context)
        {
            _context = context;
        }

		public async Task<List<AppUser>> GetRecommendationsAsync(string userId, Preference? preference, int page, int pageSize)
		{
			var currentUser = await _context.Users
					  .Include(u => u.Location)
					  .FirstOrDefaultAsync(u => u.Id == userId);

			var currentLocation = currentUser?.Location?.GeoLocation;

			var query = _context.Users
				.Include(u => u.Location)
				.Where(u => u.Id != userId)
				.Where(u => !_context.Swipes.Any(s =>
					(s.SwiperId == userId && s.TargetId == u.Id) ||
					(s.TargetId == userId && s.SwiperId == u.Id)
				));

			if (preference != null)
			{
				query = query
					.Where(u => u.Age >= preference.MinAge && u.Age <= preference.MaxAge)
					.Where(u => u.Gender == preference.PreferredGender);

				if (currentLocation != null && preference.MaxDistance > 0)
				{
					var distanceInMeters = preference.MaxDistance * 1000;
					query = query.Where(u =>
						u.Location != null &&
						u.Location.GeoLocation.Distance(currentLocation) <= distanceInMeters);
				}
			}

			query = query
				.OrderBy(u => u.UserName)
				.Skip((page - 1) * pageSize)
				.Take(pageSize);

			return await query.ToListAsync();
		}

		public async Task<Point?> GetUserLocationAsync(string userId)
		{
			return await _context.Users
				.Where(u => u.Id == userId)
				.Select(u => u.Location.GeoLocation)
				.FirstOrDefaultAsync();
		}
	}
}