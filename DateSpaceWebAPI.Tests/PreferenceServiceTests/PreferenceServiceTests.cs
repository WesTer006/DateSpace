using AutoMapper;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Moq;
using Shared.DTOs;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Tests
{
    public class PreferenceServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PreferenceService _preferenceService;

        public PreferenceServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _preferenceService = new PreferenceService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        private Preference CreateTestPreference(string userId)
        {
            return new Preference
            {
                UserId = userId,
                PreferredGender = "Any", // Обязательное поле
                MinAge = 18,
                MaxAge = 30,
                User = new AppUser
                {
                    Id = userId,
                    Gender = "Any"    // <- обязательно
                                      // если есть ещё required-поля в AppUser, заполните и их
                }
            };
        }

        private PreferenceDto CreateTestPreferenceDto(string userId)
        {
            return new PreferenceDto
            {
                UserId = userId, // Обязательное поле
                PreferredGender = "Any",
                MinAge = 18,
                MaxAge = 30
            };
        }

        [Fact]
        public async Task GetPreferencesAsync_ReturnsPreferences_WhenExists()
        {
            // Arrange
            var userId = "user1";
            var preference = CreateTestPreference(userId);
            var preferenceDto = CreateTestPreferenceDto(userId);

            var mockRepo = new Mock<IRepository<Preference>>();
            mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Preference, bool>>>()))
                .ReturnsAsync(new List<Preference> { preference });

            _mockUnitOfWork.Setup(u => u.GetRepository<Preference>()).Returns(mockRepo.Object);
            _mockMapper.Setup(m => m.Map<PreferenceDto>(preference)).Returns(preferenceDto);

            // Act
            var result = await _preferenceService.GetPreferencesAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetPreferencesAsync_ReturnsNull_WhenNotExists()
        {
            // Arrange
            var userId = "user1";
            var mockRepo = new Mock<IRepository<Preference>>();
            mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Preference, bool>>>()))
                .ReturnsAsync(new List<Preference>());

            _mockUnitOfWork.Setup(u => u.GetRepository<Preference>()).Returns(mockRepo.Object);

            // Act
            var result = await _preferenceService.GetPreferencesAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddPreferencesAsync_AddsNewPreferences()
        {
            // Arrange
            var userId = "user1";
            var preferenceDto = CreateTestPreferenceDto(userId);
            var preference = CreateTestPreference(userId);

            var mockRepo = new Mock<IRepository<Preference>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Preference>()).Returns(mockRepo.Object);
            _mockMapper.Setup(m => m.Map<Preference>(preferenceDto)).Returns(preference);
            _mockMapper.Setup(m => m.Map<PreferenceDto>(preference)).Returns(preferenceDto);

            // Act
            var result = await _preferenceService.AddPreferencesAsync(userId, preferenceDto);

            // Assert
            Assert.Equal(userId, result.UserId);
            mockRepo.Verify(r => r.AddAsync(preference), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdatePreferencesAsync_UpdatesExistingPreferences()
        {
            // Arrange
            var userId = "user1";
            var preferenceDto = CreateTestPreferenceDto(userId);
            preferenceDto.MinAge = 20;
            preferenceDto.MaxAge = 35;

            var existingPreference = CreateTestPreference(userId);

            var mockRepo = new Mock<IRepository<Preference>>();
            mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Preference, bool>>>()))
                    .ReturnsAsync(new List<Preference> { existingPreference });

            _mockUnitOfWork.Setup(u => u.GetRepository<Preference>()).Returns(mockRepo.Object);

            // 1) Настройка Map(dto, entity) для изменения сущности
            _mockMapper.Setup(m => m.Map(preferenceDto, existingPreference))
                       .Callback<PreferenceDto, Preference>((dto, entity) =>
                       {
                           entity.MinAge = dto.MinAge;
                           entity.MaxAge = dto.MaxAge;
                           entity.MaxDistance = dto.MaxDistance;
                           entity.PreferredGender = dto.PreferredGender;
                       });

            // 2) Настройка Map<PreferenceDto>(entity) чтобы вернуть DTO с обновлёнными полями
            _mockMapper.Setup(m => m.Map<PreferenceDto>(existingPreference))
                       .Returns(preferenceDto);

            // Act
            var result = await _preferenceService.UpdatePreferencesAsync(userId, preferenceDto);

            // Assert
            Assert.Equal(20, result.MinAge);
            Assert.Equal(35, result.MaxAge);
            mockRepo.Verify(r => r.Update(existingPreference), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task UpdatePreferencesAsync_Throws_WhenPreferencesNotFound()
        {
            // Arrange
            var userId = "user1";
            var preferenceDto = CreateTestPreferenceDto(userId);

            var mockRepo = new Mock<IRepository<Preference>>();
            mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Preference, bool>>>()))
                .ReturnsAsync(new List<Preference>());

            _mockUnitOfWork.Setup(u => u.GetRepository<Preference>()).Returns(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _preferenceService.UpdatePreferencesAsync(userId, preferenceDto));
        }

        [Fact]
        public async Task DeletePreferencesAsync_DeletesPreferences_WhenExists()
        {
            // Arrange
            var userId = "user1";
            var preference = CreateTestPreference(userId);

            var mockRepo = new Mock<IRepository<Preference>>();
            mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Preference, bool>>>()))
                .ReturnsAsync(new List<Preference> { preference });

            _mockUnitOfWork.Setup(u => u.GetRepository<Preference>()).Returns(mockRepo.Object);

            // Act
            var result = await _preferenceService.DeletePreferencesAsync(userId);

            // Assert
            Assert.True(result);
            mockRepo.Verify(r => r.Remove(preference), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeletePreferencesAsync_ReturnsFalse_WhenNotExists()
        {
            // Arrange
            var userId = "user1";
            var mockRepo = new Mock<IRepository<Preference>>();
            mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Preference, bool>>>()))
                .ReturnsAsync(new List<Preference>());

            _mockUnitOfWork.Setup(u => u.GetRepository<Preference>()).Returns(mockRepo.Object);

            // Act
            var result = await _preferenceService.DeletePreferencesAsync(userId);

            // Assert
            Assert.False(result);
        }
    }
}