using AutoMapper;
using DateSpaceWebAPI.Mapping;
using DataAccessLayer.Entities;
using Shared.DTOs;
using NetTopologySuite.Geometries;

namespace DateSpaceWebAPI.Tests
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
        public void MappingConfiguration_IsValid()
        {
            Assert.True(true);
        }

        [Fact]
        public void Should_Map_Location_To_LocationDto()
        {
            var point = new Point(10.5, 20.5);
            var location = new DataAccessLayer.Entities.Location
            {
                Id = 1,
                UserId = "user1",
                GeoLocation = point,
                UpdatedAt = DateTime.UtcNow,
                User = null!
            };

            var dto = _mapper.Map<LocationDto>(location);

            Assert.NotNull(dto.GeoLocation);
            Assert.Equal(point.X, dto.GeoLocation!.X);
            Assert.Equal(point.Y, dto.GeoLocation!.Y);
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
