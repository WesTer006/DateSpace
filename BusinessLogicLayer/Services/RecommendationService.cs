using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using NetTopologySuite.Geometries;
using Shared.DTOs;

namespace BusinessLogicLayer.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IRecommendationRepository _recommendationRepository;
        private readonly IMapper _mapper;

        public RecommendationService(IUnitOfWork unitOfWork, IRecommendationRepository recommendationRepository, IMapper mapper)
        {
            _recommendationRepository = recommendationRepository;
            _mapper = mapper;
        }

		public async Task<List<PublicUserDto>> GetRecommendationsAsync(
			string userId,
			PreferenceDto preferenceDto,
			int page = 1,
			int pageSize = 10)
		{
			if (page < 1 || pageSize <= 0)
				return new List<PublicUserDto>();

			var preference = _mapper.Map<Preference>(preferenceDto);
			var users = await _recommendationRepository.GetRecommendationsAsync(
				userId, preference, page, pageSize);

			var currentLocation = await _recommendationRepository.GetUserLocationAsync(userId);

			var result = new List<PublicUserDto>();
			foreach (var user in users)
			{
				var userDto = _mapper.Map<PublicUserDto>(user);
				userDto.DistanceKm = CalculateDistanceKm(currentLocation, user.Location?.GeoLocation);
				result.Add(userDto);
			}

			return result;
		}

		private static double CalculateDistanceKm(Point? point1, Point? point2)
		{
			if (point1 == null || point2 == null)
				return -1;

			const double R = 6371.0;

			double lat1 = DegreesToRadians(point1.Y);
			double lon1 = DegreesToRadians(point1.X);
			double lat2 = DegreesToRadians(point2.Y);
			double lon2 = DegreesToRadians(point2.X);

			double dLat = lat2 - lat1;
			double dLon = lon2 - lon1;

			double a = Math.Pow(Math.Sin(dLat / 2), 2) +
					   Math.Cos(lat1) * Math.Cos(lat2) *
					   Math.Pow(Math.Sin(dLon / 2), 2);

			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

			return R * c;
		}

		private static double DegreesToRadians(double degrees)
		{
			return degrees * Math.PI / 180;
		}
	}
}
