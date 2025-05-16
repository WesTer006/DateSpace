using Shared.DTOs;

namespace BusinessLogicLayer.Interfaces
{
	public interface ISwipeService
	{
		Task<SwipeResultDto> SwipeAsync(string swiperId, string targetId);
	}
}
