using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Security.Claims;

namespace DateSpaceWebAPI.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class SwipesController : ControllerBase
	{
		private readonly ISwipeService _swipesService;

		public SwipesController(ISwipeService swipesService)
		{
			_swipesService = swipesService;
		}

		[HttpPost]
		public async Task<ActionResult<SwipeResultDto>> Swipe([FromBody] string targetId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userId))
				return Unauthorized();

			if (string.IsNullOrEmpty(targetId))
				return BadRequest("Target ID is required.");

			if (userId == targetId)
				return BadRequest("You cannot swipe yourself.");

			var result = await _swipesService.SwipeAsync(userId, targetId);

			if (!result.Success)
				return BadRequest("User not found or swipe failed.");

			return Ok(result);
		}

	}
}
