using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DateSpaceWebAPI.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UserProfileController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserProfileController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet("me")]
		public async Task<IActionResult> GetMyProfile()
		{
			var userName = User.Identity?.Name;

			if (string.IsNullOrEmpty(userName))
			{
				return Unauthorized(new { message = "User is not authenticated" });
			}

			var user = await _userService.GetUserByNameAsync(userName);

			if (user == null)
			{
				return NotFound(new { message = "User not found" });
			}

			return Ok(new
			{
				user.UserName,
				user.Age,
				user.Gender,
				user.Bio,
				user.Email
			});
		}

		[HttpPut("me")]
		public async Task<IActionResult> UpdateMyProfile([FromBody] UserProfileDto user)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
				return Unauthorized(new { message = "Unauthorized" });

			// Обработка и валидация значений, переданных в UserProfileDto
			var updated = await _userService.UpdateProfileAsync(
				userId,
				user.UserName,
				user.Age,
				user.Gender,
				user.Bio,
				user.Email);  // Теперь Email тоже передается, если необходимо

			if (!updated)
				return NotFound(new { message = "User not found" });

			return Ok(new { message = "Profile updated successfully" });
		}

		[HttpPut("me/password")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier);

			ArgumentNullException.ThrowIfNull(userId);

			var success = await _userService.ChangePasswordAsync(userId.Value, dto.OldPassword, dto.NewPassword);

			if (!success)
				return BadRequest(new { message = "Password change failed. Old password might be incorrect." });

			return Ok(new { message = "Password changed successfully" });
		}
	}
}