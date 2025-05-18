using AutoMapper;
using DataAccessLayer.Entities;
using NetTopologySuite.Geometries;
using Shared.DTOs;

namespace DateSpaceWebAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, PublicUserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
				.ForMember(dest => dest.DistanceKm, opt => opt.Ignore());

            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<UserDto, AppUser>(MemberList.None)
		        .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
		        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
		        .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));

			CreateMap<DataAccessLayer.Entities.Location, LocationDto>()
				.ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.GeoLocation.Y))
				.ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.GeoLocation.X))
				.ReverseMap()
				.ForMember(dest => dest.GeoLocation,
					opt => opt.MapFrom(src => new Point(src.Longitude, src.Latitude) { SRID = 4326 }))
				.ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
				.ForMember(dest => dest.UserId, opt => opt.Ignore())
				.ForMember(dest => dest.User, opt => opt.Ignore());


			CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.SenderId))
                .ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => src.ReceiverId))
                .ForMember(dest => dest.MessageText, opt => opt.MapFrom(src => src.MessageText))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ReverseMap()
                .ForMember(dest => dest.Sender, opt => opt.Ignore())
                .ForMember(dest => dest.Receiver, opt => opt.Ignore());

            CreateMap<Photo, PhotoDto>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.IsMain, opt => opt.MapFrom(src => src.IsMain))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Preference, PreferenceDto>()
                .ForMember(dest => dest.PreferredGender, opt => opt.MapFrom(src => src.PreferredGender))
                .ForMember(dest => dest.MinAge, opt => opt.MapFrom(src => src.MinAge))
                .ForMember(dest => dest.MaxAge, opt => opt.MapFrom(src => src.MaxAge))
                .ForMember(dest => dest.MaxDistance, opt => opt.MapFrom(src => src.MaxDistance))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.PreferredGender, opt => opt.MapFrom(src => src.PreferredGender))
                .ForMember(dest => dest.MinAge, opt => opt.MapFrom(src => src.MinAge))
                .ForMember(dest => dest.MaxAge, opt => opt.MapFrom(src => src.MaxAge))
                .ForMember(dest => dest.MaxDistance, opt => opt.MapFrom(src => src.MaxDistance));

            CreateMap<Swipe, SwipeDto>()
                .ForMember(dest => dest.SwiperId, opt => opt.MapFrom(src => src.SwiperId))
                .ForMember(dest => dest.TargetId, opt => opt.MapFrom(src => src.TargetId))
                .ForMember(dest => dest.TargetAgree, opt => opt.MapFrom(src => src.TargetAgree))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ReverseMap()
                .ForMember(dest => dest.Swiper, opt => opt.Ignore())
                .ForMember(dest => dest.Target, opt => opt.Ignore());
        }
    }
}
