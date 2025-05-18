using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Shared.DTOs;

namespace BusinessLogicLayer.Services
{
	public class LocationService:ILocationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public LocationService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<LocationDto?> GetLocationAsync(string userId)
		{
			var locationRepo = _unitOfWork.GetRepository<Location>();

			var locs = await locationRepo.FindAsync(p => p.UserId == userId);
			var loc = locs.FirstOrDefault();

			return loc == null ? null : _mapper.Map<LocationDto>(loc);
		}

		public async Task<LocationDto> AddLocationAsync(string userId, LocationDto locationDto)
		{
			var entity = _mapper.Map<Location>(locationDto);
			entity.UserId = userId;

			var locationRepo = _unitOfWork.GetRepository<Location>();
			await locationRepo.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<LocationDto>(entity);
		}

		public async Task<LocationDto> UpdateLocationAsync(string userId, LocationDto locationDto)
		{
			var locationRepo = _unitOfWork.GetRepository<Location>();

			var locs = await locationRepo.FindAsync(p => p.UserId == userId);
			var entity = locs.FirstOrDefault();

			if (entity == null)
				throw new InvalidOperationException("Location not found for this user.");

			_mapper.Map(locationDto, entity);
			entity.UserId = userId;

			locationRepo.Update(entity);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<LocationDto>(entity);
		}
	}
}
