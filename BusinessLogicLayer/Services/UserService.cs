using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
	internal class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<AppUser?> AuthenticateUserAsync(string username, string password)
		{
			var user = await _userRepository.FindByEmailAsync(username);
			if (user != null && await _userRepository.CheckPasswordAsync(user, password))
			{
				return user;
			}
			return null;
		}

		public async Task UpdateUserTokensAsync(AppUser user, string refreshToken, DateTime expiryTime)
		{
			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiryTime = expiryTime;
			await _userRepository.UpdateUserAsync(user);
		}
		public async Task<AppUser?> GetUserByEmailAsync(string username)
		{
			return await _userRepository.FindByEmailAsync(username);
		}
		public async Task<IdentityResult> RegisterUserAsync(AppUser user)
		{
			return await _userRepository.CreateUserAsync(user);
		}
	}
}
