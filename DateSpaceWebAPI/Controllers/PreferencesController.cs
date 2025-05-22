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
    public class PreferencesController : ControllerBase
    {
        private readonly IPreferenceService _preferenceService;

        public PreferencesController(IPreferenceService preferenceService)
        {
            _preferenceService = preferenceService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PreferenceDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PreferenceDto>> GetPreferences()
        {
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
                return Unauthorized();

            var preferences = await _preferenceService.GetPreferencesAsync(userId);
            return preferences == null ? NotFound() : Ok(preferences);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PreferenceDto), 201)]
        public async Task<ActionResult<PreferenceDto>> AddPreferences([FromBody] PreferenceDto preferenceDto)
        {
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
                return Unauthorized();

            var preferences = await _preferenceService.AddPreferencesAsync(userId, preferenceDto);
            return CreatedAtAction(nameof(GetPreferences), null, preferences);
        }

        [HttpPut]
        [ProducesResponseType(typeof(PreferenceDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PreferenceDto>> UpdatePreferences([FromBody] PreferenceDto preferenceDto)
        {
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
                return Unauthorized();

            var preferences = await _preferenceService.UpdatePreferencesAsync(userId, preferenceDto);
            return Ok(preferences);
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePreferences()
        {
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
                return Unauthorized();

            await _preferenceService.DeletePreferencesAsync(userId);
            return NoContent();
        }
    }
}
