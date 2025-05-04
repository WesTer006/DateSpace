using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogicLayer.Interfaces
{
	public interface IUserService
	{
		Task<AppUser?> AuthenticateUserAsync(string username, string password);
		Task UpdateUserTokensAsync(AppUser user, string? refreshToken, DateTime? expiryTime);
		Task<AppUser?> GetUserByNameAsync(string username);
		Task<IdentityResult> RegisterUserAsync(AppUser user, string password);
		Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken);
		Task<bool> UpdateProfileAsync(string userId, string username, int? age, string? gender, string? bio, string? email);
		Task<AppUser?> GetUserByIdAsync(string id);
		Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
	}
}