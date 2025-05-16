using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
	public interface ISwipeRepository
	{
		Task<Swipe> CreateSwipeAsync(AppUser swiper, AppUser target);
		Task<Swipe?> GetReverseSwipeAsync(string swiperId, string targetId);
		Task SetTargetAgreeAsync(string targetId, string swiperId);
	}
}