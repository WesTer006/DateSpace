using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
	internal class TokenService:ITokenService
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
			new Claim(ClaimTypes.Name, user.UserName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

			var authSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				expires: DateTime.UtcNow.AddMinutes(15),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public string GenerateRefreshToken()
		{
			return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
		}

		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = false,
				ValidateIssuerSigningKey = true,
				ValidIssuer = _configuration["Jwt:Issuer"],
				ValidAudience = _configuration["Jwt:Audience"],
				IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]))
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
			var jwtSecurityToken = securityToken as JwtSecurityToken;

			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new SecurityTokenException("Invalid token");
			}

			return principal;
		}
	}
}