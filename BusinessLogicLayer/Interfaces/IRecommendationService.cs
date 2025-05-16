using Shared.DTOs;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<PublicUserDto>> GetRecommendationsAsync(string userId, int page = 1, int pageSize = 10);
    }
}
