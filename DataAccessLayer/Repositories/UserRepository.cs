using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
	internal class UserRepository:IUserRepository
	{
		private readonly UserManager<AppUser> _userManager;

		public UserRepository(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<AppUser?> FindByEmailAsync(string username)
		{
			return await _userManager.FindByEmailAsync(username);
		}

		public async Task<bool> CheckPasswordAsync(AppUser user, string password)
		{
			return await _userManager.CheckPasswordAsync(user, password);
		}

		public async Task UpdateUserAsync(AppUser user)
		{
			await _userManager.UpdateAsync(user);
		}
		public async Task<IdentityResult> CreateUserAsync(AppUser user)
		{
			return await _userManager.CreateAsync(user);
		}
	}
}
