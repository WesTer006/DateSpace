using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
	public interface IUserRepository
	{
		Task<AppUser?> FindByEmailAsync(string username);
		Task<bool> CheckPasswordAsync(AppUser user, string password);
		Task UpdateUserAsync(AppUser user);
		Task<IdentityResult> CreateUserAsync(AppUser user);
	}
}
