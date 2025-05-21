using DataAccessLayer.Entities;
using NetTopologySuite.Geometries;

namespace DataAccessLayer.Interfaces
{
    public interface IRecommendationRepository
    {
        Task<List<AppUser>> GetRecommendationsAsync(string userId, Preference? preference, int page, int pageSize);
        Task<Point?> GetUserLocationAsync(string userId);
	}
}