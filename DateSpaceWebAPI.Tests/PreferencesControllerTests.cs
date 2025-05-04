using BusinessLogicLayer.Interfaces;
using DateSpaceWebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.DTOs;
using System.Security.Claims;

namespace DateSpaceWebAPI.Tests.Controllers
{
    public class PreferencesControllerTests
    {
        private readonly Mock<IPreferenceService> _mockPreferenceService;
        private readonly PreferencesController _controller;

        public PreferencesControllerTests()
        {
            _mockPreferenceService = new Mock<IPreferenceService>();
            _controller = new PreferencesController(_mockPreferenceService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        private void SetupAuthenticatedUser()
        {
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "authenticated_user_id") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(identity);
        }

        [Fact]
        public async Task GetPreferences_ReturnsOk_WhenPreferencesExist()
        {
            SetupAuthenticatedUser();
            var expectedDto = new PreferenceDto { MinAge = 18, MaxAge = 30 };

            _mockPreferenceService.Setup(x => x.GetPreferencesAsync("authenticated_user_id"))
                .ReturnsAsync(expectedDto);

            var result = await _controller.GetPreferences();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedDto, okResult.Value);
        }

        [Fact]
        public async Task AddPreferences_ReturnsCreated_WhenValid()
        {
            SetupAuthenticatedUser();
            var inputDto = new PreferenceDto { MinAge = 18, MaxAge = 30 };
            var expectedDto = new PreferenceDto { MinAge = 18, MaxAge = 30 };

            _mockPreferenceService.Setup(x => x.AddPreferencesAsync(
                "authenticated_user_id",
                It.Is<PreferenceDto>(dto =>
                    dto.MinAge == 18 &&
                    dto.MaxAge == 30)
            )).ReturnsAsync(expectedDto);

            var result = await _controller.AddPreferences(inputDto);

            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(PreferencesController.GetPreferences), createdAtResult.ActionName);
            Assert.Equal(expectedDto, createdAtResult.Value);
        }

        [Fact]
        public async Task UpdatePreferences_ReturnsOk_WhenUpdated()
        {
            SetupAuthenticatedUser();
            var inputDto = new PreferenceDto { MinAge = 20, MaxAge = 35 };

            _mockPreferenceService.Setup(x => x.UpdatePreferencesAsync(
                "authenticated_user_id",
                It.Is<PreferenceDto>(dto =>
                    dto.MinAge == 20 &&
                    dto.MaxAge == 35)
            )).ReturnsAsync(inputDto);

            var result = await _controller.UpdatePreferences(inputDto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(inputDto, okResult.Value);
        }

        [Fact]
        public async Task DeletePreferences_ReturnsNoContent_WhenDeleted()
        {
            SetupAuthenticatedUser();
            _mockPreferenceService.Setup(x => x.DeletePreferencesAsync("authenticated_user_id"))
                .ReturnsAsync(true);

            var result = await _controller.DeletePreferences();

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AllMethods_ReturnUnauthorized_WhenUserNotAuthenticated()
        {
            var dto = new PreferenceDto { MinAge = 18, MaxAge = 30 };

            var getResult = await _controller.GetPreferences();
            Assert.IsType<UnauthorizedResult>(getResult.Result);

            var addResult = await _controller.AddPreferences(dto);
            Assert.IsType<UnauthorizedResult>(addResult.Result);

            var updateResult = await _controller.UpdatePreferences(dto);
            Assert.IsType<UnauthorizedResult>(updateResult.Result);

            var deleteResult = await _controller.DeletePreferences();
            Assert.IsType<UnauthorizedResult>(deleteResult);
        }
    }
}