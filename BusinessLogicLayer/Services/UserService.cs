using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace BusinessLogicLayer.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
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
	}
}
