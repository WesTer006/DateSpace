using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BusinessLogicLayer
{
	public static class ServiceExtension
	{
		public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services,
		IConfiguration configuration)
		{
			return services.AddIdentity();
		}
		private static IServiceCollection AddIdentity(this IServiceCollection services)
		{
			services.AddIdentity<AppUser, IdentityRole>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequiredLength = 8;
			}).AddEntityFrameworkStores<AppDbContext>();

			return services;
		}
	}
}
