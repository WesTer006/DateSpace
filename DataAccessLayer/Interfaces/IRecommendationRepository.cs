using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IRecommendationRepository
    {
        Task<List<AppUser>> GetRecommendationsAsync(string userId, Preference? preference, int page, int pageSize);
    }
}