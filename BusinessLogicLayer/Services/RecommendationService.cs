using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Shared.DTOs;

namespace BusinessLogicLayer.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRecommendationRepository _recommendationRepository;
        private readonly IMapper _mapper;

        public RecommendationService(IUnitOfWork unitOfWork, IRecommendationRepository recommendationRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _recommendationRepository = recommendationRepository;
            _mapper = mapper;
        }

        public async Task<List<PublicUserDto>> GetRecommendationsAsync(string userId, int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize <= 0)
                return new List<PublicUserDto>();

            var preferenceRepo = _unitOfWork.GetRepository<Preference>();
            var preferences = await preferenceRepo.FindAsync(p => p.UserId == userId);
            var preference = preferences.FirstOrDefault();

            var filteredUsers = await _recommendationRepository.GetRecommendationsAsync(userId, preference, page, pageSize);

            return _mapper.Map<List<PublicUserDto>>(filteredUsers);
        }
    }
}