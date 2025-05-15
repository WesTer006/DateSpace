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
                .AsNoTracking() // Отключаем отслеживание для оптимизации
                .Where(u => u.Id != userId) // Исключаем текущего пользователя
                .Where(u => !_context.Swipes.Any(s =>
                    (s.SwiperId == userId && s.TargetId == u.Id) ||
                    (s.TargetId == userId && s.SwiperId == u.Id)
                )); // Исключаем просвайпанных

            // Фильтрация по предпочтениям
            if (preference != null)
            {
                query = query
                    .Where(u => u.Age >= preference.MinAge && u.Age <= preference.MaxAge)
                    .Where(u => u.Gender == preference.PreferredGender);
                // TODO: MaxDistance (заглушка, фильтр добавим позже)
            }

            // Пагинация
            query = query
                .OrderBy(u => u.UserName) // Для детерминированного порядка
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }
    }
}