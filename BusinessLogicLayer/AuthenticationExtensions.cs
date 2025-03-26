using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BusinessLogicLayer
{
	public static class AuthenticationExtensions
	{
		public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
		{
			return services.AddIdentityServices()
						   .AddJwtAuthentication(configuration);
		}

		// 🔹 Настраиваем Identity
		private static IServiceCollection AddIdentityServices(this IServiceCollection services)
		{
			services.AddIdentity<AppUser, IdentityRole>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequiredLength = 8;
			})
			.AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders() // 🔹 Добавляем генерацию токенов (для refresh, reset password и т. д.)
			.AddSignInManager<SignInManager<AppUser>>(); // 🔹 Добавляем SignInManager

			return services;
		}

		// 🔹 Настраиваем JWT-аутентификацию
		private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			var jwtSettings = configuration.GetSection("JwtOptions");
			var secretKey = jwtSettings.GetValue<string>("SecretKey");

			if (string.IsNullOrEmpty(secretKey))
			{
				throw new ArgumentNullException("SecretKey", "JWT Secret Key is missing in the configuration.");
			}

			var key = Encoding.ASCII.GetBytes(secretKey);

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
						ValidAudience = jwtSettings.GetValue<string>("Audience"),
						IssuerSigningKey = new SymmetricSecurityKey(key)
					};
				});

			// 🔹 Регистрируем сервис токенов
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IUserService, UserService>();
			return services;
		}
	}
}
