using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly AppDbContext _context;

        public RecommendationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppUser>> GetRecommendationsAsync(string userId, Preference? preference, int page, int pageSize)
        {
            var query = _context.Users
                .AsNoTracking()
                .Where(u => u.Id != userId)
                .Where(u => !_context.Swipes.Any(s =>
                    (s.SwiperId == userId && s.TargetId == u.Id) ||
                    (s.TargetId == userId && s.SwiperId == u.Id)
                ));

            if (preference != null)
            {
                query = query
                    .Where(u => u.Age >= preference.MinAge && u.Age <= preference.MaxAge)
                    .Where(u => u.Gender == preference.PreferredGender);
                // TODO: MaxDistance (заглушка, фильтр добавим позже)
            }

            query = query
                .OrderBy(u => u.UserName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }
    }
}