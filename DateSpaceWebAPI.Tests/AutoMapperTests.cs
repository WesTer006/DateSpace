using AutoMapper;
using DataAccessLayer.Entities;
using DateSpaceWebAPI.Mapping;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using DateSpaceWebAPI.Mapping;
using Shared.DTOs;

namespace Shared.Tests
{
    public class MappingTests
    {
        private readonly IMapper _mapper;

        public MappingTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Should_Map_AppUser_To_PublicUserDto()
        {
            // Arrange
            var appUser = new AppUser
            {
                UserName = "masha",
                Age = 19,
                Gender = "Female",
                Bio = "biobiobio",
                Email = "masha@gmail.com",
                Id = "user123",
                Preference = null,
                Location = null,
                Photos = [],
                RefreshToken = null,
                RefreshTokenExpiryTime = null
            };

            // Act
            var dto = _mapper.Map<PublicUserDto>(appUser);

            // Assert
            Assert.Equal(appUser.UserName, dto.UserName);
            Assert.Equal(appUser.Age, dto.Age);
            Assert.Equal(appUser.Gender, dto.Gender);
            Assert.Equal(appUser.Bio, dto.Bio);
        }

        [Fact]
        public void MappingConfiguration_IsValid()
        {
            Assert.True(true);
        }

        [Fact]
        public void Should_Map_AppUser_To_UserDto()
        {
            // Arrange
            var appUser = new AppUser
            {
                UserName = "testuser",
                Age = 25,
                Gender = "Male",
                Bio = "Just a cool guy",
                Email = "testuser@example.com",
                Id = "user1",
                Preference = null,
                Location = null,
                Photos = [],
                RefreshToken = null,
                RefreshTokenExpiryTime = null
            };

            // Act
            var dto = _mapper.Map<UserDto>(appUser);

            // Assert
            Assert.Equal(appUser.UserName, dto.UserName);
            Assert.Equal(appUser.Age, dto.Age);
            Assert.Equal(appUser.Gender, dto.Gender);
            Assert.Equal(appUser.Bio, dto.Bio);
            Assert.Equal(appUser.Email, dto.Email);
            Assert.Null(dto.Password);
        }

		[Fact]
		public void Should_Map_Location_To_LocationDto()
		{
			// Arrange
			var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
			var point = geometryFactory.CreatePoint(new Coordinate( 10.5,  20.5));

			var location = new DataAccessLayer.Entities.Location
			{
				Id = 1,
				UserId = "user1",
				GeoLocation = point,
				UpdatedAt = DateTime.UtcNow,
				User = null!
			};

			// Act
			var dto = _mapper.Map<LocationDto>(location);

			// Assert
			Assert.Equal(20.5, dto.Latitude);  // Y = latitude
			Assert.Equal(10.5, dto.Longitude); // X = longitude
		}

		[Fact]
		public void Should_Map_LocationDto_To_Location()
		{
			// Arrange
			var dto = new LocationDto
			{
				Latitude = 20.5,
				Longitude = 10.5,
			};

			// Act
			var location = _mapper.Map<DataAccessLayer.Entities.Location>(dto);

			// Assert
			Assert.Equal(10.5, location.GeoLocation.X);    // longitude
			Assert.Equal(20.5, location.GeoLocation.Y);    // latitude
			Assert.Equal(4326, location.GeoLocation.SRID); // Проверяем SRID
		}


		[Fact]
        public void Should_Map_Message_To_MessageDto()
        {
            var now = DateTime.UtcNow;
            var message = new Message
            {
                Id = 1,
                SenderId = "user1",
                ReceiverId = "user2",
                MessageText = "Привет, как дела?",
                CreatedAt = now,
                Sender = null!,
                Receiver = null!
            };

            var dto = _mapper.Map<MessageDto>(message);

            Assert.Equal(message.SenderId, dto.SenderId);
            Assert.Equal(message.ReceiverId, dto.ReceiverId);
            Assert.Equal(message.MessageText, dto.MessageText);
            Assert.Equal(message.CreatedAt, dto.CreatedAt);
        }

        [Fact]
        public void Should_Map_Photo_To_PhotoDto()
        {
            var photo = new Photo
            {
                Id = 1,
                UserId = "user1",
                Url = "https://example.com/photo.jpg",
                IsMain = true,
                User = null!
            };

            var dto = _mapper.Map<PhotoDto>(photo);

            Assert.Equal(photo.Url, dto.Url);
            Assert.Equal(photo.IsMain, dto.IsMain);
        }

        [Fact]
        public void Should_Map_Preference_To_PreferenceDto()
        {
            var preference = new Preference
            {
                Id = 1,
                UserId = "user1",
                PreferredGender = "Female",
                MinAge = 18,
                MaxAge = 30,
                MaxDistance = 100,
                User = null!
            };

            var dto = _mapper.Map<PreferenceDto>(preference);

            Assert.Equal(preference.PreferredGender, dto.PreferredGender);
            Assert.Equal(preference.MinAge, dto.MinAge);
            Assert.Equal(preference.MaxAge, dto.MaxAge);
            Assert.Equal(preference.MaxDistance, dto.MaxDistance);
        }


        [Fact]
        public void Should_Map_Swipe_To_SwipeDto()
        {
            var now = DateTime.UtcNow;
            var swipe = new Swipe
            {
                SwiperId = "user1",
                TargetId = "user2",
                TargetAgree = true,
                CreatedAt = now,
                Swiper = null!,
                Target = null!
            };

            var dto = _mapper.Map<SwipeDto>(swipe);

            Assert.Equal(swipe.SwiperId, dto.SwiperId);
            Assert.Equal(swipe.TargetId, dto.TargetId);
            Assert.Equal(swipe.TargetAgree, dto.TargetAgree);
            Assert.Equal(swipe.CreatedAt, dto.CreatedAt);
        }
    }
}
