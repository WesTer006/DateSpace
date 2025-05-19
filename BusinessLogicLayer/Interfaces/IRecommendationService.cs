using DataAccessLayer.Entities;
using Shared.DTOs;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<PublicUserDto>> GetRecommendationsAsync(string userId, PreferenceDto preferenceDto, int page = 1, int pageSize = 10);
	}
}
