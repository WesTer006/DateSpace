using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs;

namespace BusinessLogicLayer.Services
{
	public class UserService : IUserService
	{
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AppUser?> AuthenticateUserAsync(string username, string password)
		{
			var user = await _userRepository.FindByNameAsync(username);
			if (user != null && await _userRepository.CheckPasswordAsync(user, password))
			{
				return user;
			}
			return null;
		}

		public async Task UpdateUserTokensAsync(AppUser user, string? refreshToken, DateTime? expiryTime)
		{
			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiryTime = expiryTime;
			await _userRepository.UpdateUserAsync(user);
		}

		public async Task<AppUser?> GetUserByNameAsync(string username)
		{
			return await _userRepository.FindByNameAsync(username);
		}
		public async Task<IdentityResult> RegisterUserAsync(AppUser user, string password)
		{
			return await _userRepository.CreateUserAsync(user,password);
		}

		public async Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken)
		{
			return await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
		}

		public async Task<bool> UpdateProfileAsync(string userId, string? username, int? age, string? gender, string? bio, string? email)
		{
			var user = await _userRepository.FindByIdAsync(userId);
			if (user == null)
				return false;

			if (!string.IsNullOrEmpty(username))
				user.UserName = username;
			if (age.HasValue)
				user.Age = age.Value;
			if (!string.IsNullOrEmpty(gender))
				user.Gender = gender;
			if (!string.IsNullOrEmpty(bio))
				user.Bio = bio;
			if (!string.IsNullOrEmpty(email))
				user.Email = email;

			await _userRepository.UpdateUserAsync(user);
			return true;
		}

		public async Task<AppUser?> GetUserByIdAsync(string id)
		{
			return await _userRepository.FindByIdAsync(id);
		}

		public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
		{
			var user = await _userRepository.FindByIdAsync(userId);
			if (user == null)
				return false;

			var result = await _userRepository.ChangePasswordAsync(user, oldPassword, newPassword);
			return result.Succeeded;
		}

        public async Task<List<UserDto>> GetRecommendationsAsync(string userId, int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize <= 0)
                return new List<UserDto>();

            var preferenceRepo = _unitOfWork.GetRepository<Preference>();
            var preferences = await preferenceRepo.FindAsync(p => p.UserId == userId);
            var preference = preferences.FirstOrDefault();

            var swipeRepo = _unitOfWork.GetRepository<Swipe>();
            var swipes = await swipeRepo.FindAsync(s => s.SwiperId == userId || s.TargetId == userId);
            var swipedUserIds = swipes
                .Select(s => s.SwiperId == userId ? s.TargetId : s.SwiperId)
                .Distinct()
                .ToHashSet();

            var userRepo = _unitOfWork.GetRepository<AppUser>();
            var allUsers = await userRepo.FindAsync(u => u.Id != userId);

            var filteredUsers = allUsers
                .Where(u => !swipedUserIds.Contains(u.Id))
                .Where(u => preference == null || (
                    u.Age >= preference.MinAge &&
                    u.Age <= preference.MaxAge &&
                    u.Gender == preference.PreferredGender
                // TODO: расстояние
                ))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return _mapper.Map<List<UserDto>>(filteredUsers);
        }

    }
}
