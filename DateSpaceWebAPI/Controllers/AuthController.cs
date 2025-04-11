using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Shared.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace DateSpaceWebAPI.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;

		public AuthController(IUserService userService,
							  ITokenService tokenService,
							  IMapper mapper)
		{
			_userService = userService;
			_tokenService = tokenService;
			_mapper = mapper;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto model)
		{
			var authenticatedUser = await _userService.AuthenticateUserAsync(model.UserName, model.Password);
			if (authenticatedUser == null)
			{
				return Unauthorized();
			}

			var jwtToken = _tokenService.GenerateJwtToken(authenticatedUser);
			var refreshToken = _tokenService.GenerateRefreshToken();
			await _userService.UpdateUserTokensAsync(authenticatedUser, refreshToken, DateTime.UtcNow.AddDays(7));

			Response.Cookies.Append("jwt", jwtToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = DateTime.UtcNow.AddMinutes(15)
			});

			Response.Cookies.Append("refresh", refreshToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = DateTime.UtcNow.AddDays(7)
			});

			return Ok(new { accessToken = jwtToken, refreshToken });
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] UserDto userDto)
		{
			var user = _mapper.Map<AppUser>(userDto);
			var result = await _userService.RegisterUserAsync(user, userDto.Password);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}

			var jwtToken = _tokenService.GenerateJwtToken(user);
			var refreshToken = _tokenService.GenerateRefreshToken();
			var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

			await _userService.UpdateUserTokensAsync(user, refreshToken, DateTime.UtcNow.AddDays(7));
			Response.Cookies.Append("jwt", jwtToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = DateTime.UtcNow.AddMinutes(15)
			});

			Response.Cookies.Append("refresh", refreshToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = refreshTokenExpiry
			});
			return Ok(new { accessToken = jwtToken, refreshToken });
		}

		[HttpPost("refresh")]
		public async Task<IActionResult> RefreshToken([FromHeader(Name = "Authorization")] string authHeader)
		{
			if (string.IsNullOrEmpty(authHeader))
			{
				return Unauthorized(new { message = "Authorization header missing" });
			}

			if (!authHeader.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
			{
				return Unauthorized(new { message = "Authorization header must start with 'Bearer '" });
			}

			var refreshToken = authHeader.Substring("Bearer ".Length).Trim();

			if (string.IsNullOrEmpty(refreshToken))
			{
				return Unauthorized(new { message = "Refresh token missing" });
			}

			var user = await _userService.GetUserByRefreshTokenAsync(refreshToken);
			if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
			{
				return Unauthorized(new { message = "Invalid or expired refresh token" });
			}

			var newJwtToken = _tokenService.GenerateJwtToken(user);

			Response.Cookies.Append("jwt", newJwtToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = DateTime.UtcNow.AddMinutes(15)
			});

			return Ok(new { AccessToken = newJwtToken });
		}



		[HttpPost("logout")]
		public async Task<IActionResult> Logout([FromHeader(Name = "Authorization")] string authHeader)
		{
			if (string.IsNullOrEmpty(authHeader))
			{
				return Unauthorized(new { message = "Authorization header missing" });
			}

			if (!authHeader.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
			{
				return Unauthorized(new { message = "Authorization header must start with 'Bearer '" });
			}

			var refreshToken = authHeader.Substring("Bearer ".Length).Trim();

			if (string.IsNullOrEmpty(refreshToken))
			{
				return Unauthorized(new { message = "Refresh token missing" });
			}

			var user = await _userService.GetUserByRefreshTokenAsync(refreshToken);
			if (user == null)
			{
				return Unauthorized(new { message = "Invalid refresh token" });
			}

			await _userService.UpdateUserTokensAsync(user, null, null);

			// Remove the cookies for both JWT and refresh token on logout
			Response.Cookies.Delete("jwt");
			Response.Cookies.Delete("refresh");

			return Ok(new { message = "Logged out successfully" });
		}
	}
}