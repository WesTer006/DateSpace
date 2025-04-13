using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
				user.Bio
				
			});
		}
	}
}