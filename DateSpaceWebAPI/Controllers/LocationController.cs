using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Security.Claims;

namespace DateSpaceWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LocationController : ControllerBase
	{
		private readonly ILocationService _locationService;

		public LocationController(ILocationService locationService)
		{
			_locationService = locationService;
		}

		[HttpGet]
		[ProducesResponseType(typeof(LocationDto), 200)]
		[ProducesResponseType(404)]
		public async Task<ActionResult<LocationDto>> GetLocation()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized();

			var location = await _locationService.GetLocationAsync(userId);
			return location == null ? NotFound() : Ok(location);
		}

		[HttpPost]
		[ProducesResponseType(typeof(LocationDto), 201)]
		public async Task<ActionResult<LocationDto>> AddLocation([FromBody] LocationDto locationDto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized();

			var location = await _locationService.AddLocationAsync(userId, locationDto);
			return CreatedAtAction(nameof(GetLocation), null, location);
		}

		[HttpPut]
		[ProducesResponseType(typeof(LocationDto), 200)]
		[ProducesResponseType(404)]
		public async Task<ActionResult<LocationDto>> UpdateLocation([FromBody] LocationDto locationDto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized();

			var location = await _locationService.UpdateLocationAsync(userId, locationDto);
			return Ok(location);
		}
	}
}
