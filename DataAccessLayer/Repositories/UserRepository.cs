using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
	internal class UserRepository:IUserRepository
	{
		private readonly UserManager<AppUser> _userManager;

		public UserRepository(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<AppUser?> FindByNameAsync(string username)
		{
			return await _userManager.FindByNameAsync(username);
		}

		public async Task<bool> CheckPasswordAsync(AppUser user, string password)
		{
			return await _userManager.CheckPasswordAsync(user, password);
		}

		public async Task UpdateUserAsync(AppUser user)
		{
			await _userManager.UpdateAsync(user);
		}
		public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
		{
			user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, password);
			return await _userManager.CreateAsync(user);
		}
		public async Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken)
		{
			return await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
		}
		public async Task<AppUser?> FindByIdAsync(string id)
		{
			return await _userManager.FindByIdAsync(id);
		}
		public async Task<IdentityResult> ChangePasswordAsync(AppUser user, string oldPassword, string newPassword)
		{
			return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
		}

	}
}
