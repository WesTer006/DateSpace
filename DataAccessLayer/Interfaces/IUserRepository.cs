using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Interfaces
{
	public interface IUserRepository
	{
		Task<AppUser?> FindByNameAsync(string username);
		Task<bool> CheckPasswordAsync(AppUser user, string password);
		Task UpdateUserAsync(AppUser user);
		Task<IdentityResult> CreateUserAsync(AppUser user, string password);
		Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken);
		Task<AppUser?> FindByIdAsync(string id);
		Task<IdentityResult> ChangePasswordAsync(AppUser user, string oldPassword, string newPassword);
    }
}
