using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DateSpaceWebAPI.Extensions
{
	public static class AuthenticationExtensions
	{
		public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			var jwtSettings = configuration.GetSection("JwtOptions");

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options =>
				{
					options.RequireHttpsMetadata = false;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = false,
						ValidateAudience = false,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
					};

					options.Events = new JwtBearerEvents
					{

						OnMessageReceived = context =>
						{
							var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

							if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
							{
								context.Token = authorizationHeader.Substring("Bearer ".Length).Trim();
							}
							return Task.CompletedTask;
						},


						OnAuthenticationFailed = context =>
						{
							return Task.CompletedTask;
						},


					};
				});

			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IUserService, UserService>();

			services.AddAuthorization();
			return services;
		}
	}
}
