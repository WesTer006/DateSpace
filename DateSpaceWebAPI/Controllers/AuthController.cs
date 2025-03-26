using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DateSpaceWebAPI.DTOs;
using Microsoft.AspNetCore.Identity;
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
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			var authenticatedUser = await _userService.AuthenticateUserAsync(model.Email, model.Password);
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

			return Ok(new { Message = "Authenticated" });
		}


		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] UserDTO userDto)
		{
			var user = _mapper.Map<AppUser>(userDto);
			var result = await _userService.RegisterUserAsync(user);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}
			return Ok(new { Message = "User registered successfully" });
		}

		[HttpPost("refresh")]
		public async Task<IActionResult> RefreshToken()
		{
			if (!Request.Cookies.TryGetValue("refresh", out var refreshToken))
			{
				return Unauthorized();
			}

			var jwtToken = Request.Cookies["jwt"];
			var principal = _tokenService.GetPrincipalFromExpiredToken(jwtToken);
			var username = principal.Identity.Name;
			var user = await _userService.GetUserByEmailAsync(username);

			if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
			{
				return Unauthorized();
			}

			var newJwtToken = _tokenService.GenerateJwtToken(user);
			var newRefreshToken = _tokenService.GenerateRefreshToken();

			await _userService.UpdateUserTokensAsync(user, newRefreshToken, DateTime.UtcNow.AddDays(7));

			Response.Cookies.Append("jwt", newJwtToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = DateTime.UtcNow.AddMinutes(15)
			});

			Response.Cookies.Append("refresh", newRefreshToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = DateTime.UtcNow.AddDays(7)
			});

			return Ok(new { Message = "Token refreshed" });
		}

	}
}
