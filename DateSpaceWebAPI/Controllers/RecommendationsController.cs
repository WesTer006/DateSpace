using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Security.Claims;

namespace DateSpaceWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;
        private readonly IPreferenceService _preferenceService;
		private readonly ILocationService _locationService;

		public RecommendationsController(IRecommendationService recommendationService, IPreferenceService preferenceService, ILocationService locationService)
        {
            _recommendationService = recommendationService;
            _preferenceService = preferenceService;
            _locationService = locationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PublicUserDto>), 200)]
        public async Task<ActionResult<List<PublicUserDto>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

			var preferences = await _preferenceService.GetPreferencesAsync(userId);
			
			if (preferences == null)
                return NotFound("Preferences required");

            var location = await _locationService.GetLocationAsync(userId);

			if (location == null)
				return NotFound("Location required");

			var recommendations = await _recommendationService.GetRecommendationsAsync(userId,preferences, page, pageSize);
            return Ok(recommendations);
        }
    }
}
