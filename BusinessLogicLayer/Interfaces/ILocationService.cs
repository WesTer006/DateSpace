using Shared.DTOs;

namespace BusinessLogicLayer.Interfaces
{
	public interface ILocationService
	{
		Task<LocationDto?> GetLocationAsync(string userId);
		Task<LocationDto> AddLocationAsync(string userId, LocationDto locationDto);
		Task<LocationDto> UpdateLocationAsync(string userId, LocationDto locationDto);
	}
}
