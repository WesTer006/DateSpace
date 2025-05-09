using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Security.Claims;
using AutoMapper;

namespace DateSpaceWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecommendationsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public RecommendationsController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<UserDto>), 200)]
        public async Task<ActionResult<List<UserDto>>> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var recommendations = await _userService.GetRecommendationsAsync(userId);
            var result = _mapper.Map<List<UserDto>>(recommendations);

            return Ok(result);
        }
    }
}
