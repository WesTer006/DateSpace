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

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(PreferenceDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PreferenceDto>> GetPreferences(string userId)
        {
            if (userId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            var preferences = await _preferenceService.GetPreferencesAsync(userId);
            return preferences == null ? NotFound() : Ok(preferences);
        }

        [HttpPost("{userId}")]
        [ProducesResponseType(typeof(PreferenceDto), 201)]
        public async Task<ActionResult<PreferenceDto>> AddPreferences(
            string userId,
            [FromBody] PreferenceDto preferenceDto)
        {
            var preferences = await _preferenceService.AddPreferencesAsync(userId, preferenceDto);
            return CreatedAtAction(nameof(GetPreferences), new { userId }, preferences);
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(typeof(PreferenceDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PreferenceDto>> UpdatePreferences(
            string userId,
            [FromBody] PreferenceDto preferenceDto)
        {
            var preferences = await _preferenceService.UpdatePreferencesAsync(userId, preferenceDto);
            return Ok(preferences);
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePreferences(string userId)
        {
            await _preferenceService.DeletePreferencesAsync(userId);
            return NoContent();
        }
    }
}