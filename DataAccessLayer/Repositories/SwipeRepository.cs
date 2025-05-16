
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
	public class SwipeRepository : GenericRepository<Swipe>, ISwipeRepository
	{
		public SwipeRepository(AppDbContext context) : base(context)
		{
		}
		public async Task<Swipe?> GetReverseSwipeAsync(string swiperId, string targetId)
		{
			return await _dbSet.FirstOrDefaultAsync(s =>
				s.SwiperId == swiperId && s.TargetId == targetId);
		}

		public async Task<Swipe> CreateSwipeAsync(AppUser swiper, AppUser target)
		{
			var swipe = new Swipe
			{
				SwiperId = swiper.Id,
				TargetId = target.Id,
				Swiper = swiper,
				Target = target,
				TargetAgree=null,
				CreatedAt = DateTime.UtcNow
			};

			await _dbSet.AddAsync(swipe);
			return swipe;
		}

		public async Task SetTargetAgreeAsync(string swiperId, string targetId)
		{
			var swipe = await _dbSet.FirstOrDefaultAsync(s =>
				s.SwiperId == swiperId && s.TargetId == targetId);

			if (swipe != null)
			{
				swipe.TargetAgree = true;
				_dbSet.Update(swipe);
			}
		}
	}
}
