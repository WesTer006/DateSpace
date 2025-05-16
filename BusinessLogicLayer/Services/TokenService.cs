using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogicLayer.Services
{
	public class TokenService:ITokenService
	{
		private readonly IConfiguration _configuration;

		public TokenService(IConfiguration configuration)
		{
			_configuration = configuration;
			
		}

		public string GenerateJwtToken(AppUser user)
		{
			var authClaims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, user.UserName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

			var authSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_configuration["JwtOptions:SecretKey"]));

			var token = new JwtSecurityToken(
				issuer: _configuration["JwtOptions:Issuer"],
				audience: _configuration["JwtOptions:Audience"],
				expires: DateTime.UtcNow.AddMinutes(15),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public string GenerateRefreshToken()
		{
			return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
		}

	}
}