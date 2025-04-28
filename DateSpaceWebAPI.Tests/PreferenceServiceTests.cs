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
        private const string TestUserId = "test_user_id";

        public PreferenceServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _preferenceService = new PreferenceService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        private Preference CreateTestPreference()
        {
            return new Preference
            {
                UserId = TestUserId,
                PreferredGender = "Any",
                MinAge = 18,
                MaxAge = 30,
                MaxDistance = 100,
                User = new AppUser
                {
                    Id = TestUserId,
                    Gender = "Any",
                    UserName = "testuser"
                }
            };
        }

        private PreferenceDto CreateTestPreferenceDto()
        {
            return new PreferenceDto
            {
                PreferredGender = "Any",
                MinAge = 18,
                MaxAge = 30,
                MaxDistance = 100
            };
        }

        [Fact]
        public async Task GetPreferencesAsync_ReturnsPreferences_WhenExists()
        {
            var preference = CreateTestPreference();
            var preferenceDto = CreateTestPreferenceDto();

            var mockRepo = new Mock<IRepository<Preference>>();
            mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Preference, bool>>>()))
                .ReturnsAsync(new List<Preference> { preference });

            _mockUnitOfWork.Setup(u => u.GetRepository<Preference>()).Returns(mockRepo.Object);
            _mockMapper.Setup(m => m.Map<PreferenceDto>(preference)).Returns(preferenceDto);

            var result = await _preferenceService.GetPreferencesAsync(TestUserId);

            Assert.NotNull(result);
            Assert.Equal(preferenceDto.PreferredGender, result.PreferredGender);
        }

        [Fact]
        public async Task AddPreferencesAsync_AddsNewPreferences()
        {
            var preferenceDto = CreateTestPreferenceDto();
            var preference = CreateTestPreference();

            var mockRepo = new Mock<IRepository<Preference>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Preference>()).Returns(mockRepo.Object);
            _mockMapper.Setup(m => m.Map<Preference>(preferenceDto))
                .Returns(preference);
            _mockMapper.Setup(m => m.Map<PreferenceDto>(preference))
                .Returns(preferenceDto);

            var result = await _preferenceService.AddPreferencesAsync(TestUserId, preferenceDto);

            Assert.Equal(preferenceDto.PreferredGender, result.PreferredGender);
            mockRepo.Verify(r => r.AddAsync(It.Is<Preference>(p => p.UserId == TestUserId)), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdatePreferencesAsync_UpdatesExistingPreferences()
        {
            var preferenceDto = CreateTestPreferenceDto();
            preferenceDto.MinAge = 20;
            preferenceDto.MaxAge = 35;

            var existingPreference = CreateTestPreference();

            var mockRepo = new Mock<IRepository<Preference>>();
            mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Preference, bool>>>()))
                    .ReturnsAsync(new List<Preference> { existingPreference });

            _mockUnitOfWork.Setup(u => u.GetRepository<Preference>()).Returns(mockRepo.Object);

            _mockMapper.Setup(m => m.Map(preferenceDto, existingPreference))
                       .Callback<PreferenceDto, Preference>((dto, entity) =>
                       {
                           entity.MinAge = dto.MinAge;
                           entity.MaxAge = dto.MaxAge;
                           entity.MaxDistance = dto.MaxDistance;
                           entity.PreferredGender = dto.PreferredGender;
                       });

            _mockMapper.Setup(m => m.Map<PreferenceDto>(existingPreference))
                       .Returns(preferenceDto);

            var result = await _preferenceService.UpdatePreferencesAsync(TestUserId, preferenceDto);

            Assert.Equal(20, result.MinAge);
            Assert.Equal(35, result.MaxAge);
            mockRepo.Verify(r => r.Update(existingPreference), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeletePreferencesAsync_DeletesPreferences_WhenExists()
        {
            var preference = CreateTestPreference();

            var mockRepo = new Mock<IRepository<Preference>>();
            mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Preference, bool>>>()))
                .ReturnsAsync(new List<Preference> { preference });

            _mockUnitOfWork.Setup(u => u.GetRepository<Preference>()).Returns(mockRepo.Object);

            var result = await _preferenceService.DeletePreferencesAsync(TestUserId);

            Assert.True(result);
            mockRepo.Verify(r => r.Remove(It.Is<Preference>(p => p.UserId == TestUserId)), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
