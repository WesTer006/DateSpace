using AutoMapper;
using DataAccessLayer.Entities;
using DateSpaceWebAPI.DTOs;

namespace DateSpaceWebAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // AppUser <-> UserDTO
            CreateMap<AppUser, UserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Convert.ToInt32(src.Id)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos))
                .ForMember(dest => dest.Preference, opt => opt.MapFrom(src => src.Preference))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location));

            CreateMap<UserDTO, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio));
            // Для коллекций обратного маппинга оставляем дефолтное поведение

            // Для устранения неоднозначности используем явное указание типа сущности Location
            CreateMap<DataAccessLayer.Entities.Location, LocationDTO>()
                .ForMember(dest => dest.GeoLocation, opt => opt.MapFrom(src => src.GeoLocation));
            CreateMap<LocationDTO, DataAccessLayer.Entities.Location>()
                .ForMember(dest => dest.GeoLocation, opt => opt.MapFrom(src => src.GeoLocation));

            // Message <-> MessageDTO
            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => Convert.ToInt32(src.SenderId)))
                .ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => Convert.ToInt32(src.ReceiverId)))
                .ForMember(dest => dest.MessageText, opt => opt.MapFrom(src => src.MessageText))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
            CreateMap<MessageDTO, Message>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.SenderId.ToString()))
                .ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => src.ReceiverId.ToString()))
                .ForMember(dest => dest.MessageText, opt => opt.MapFrom(src => src.MessageText))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            // Photo <-> PhotoDTO
            CreateMap<Photo, PhotoDTO>();
            CreateMap<PhotoDTO, Photo>();

            // Preference <-> PreferenceDTO
            CreateMap<Preference, PreferenceDTO>()
                .ForMember(dest => dest.MinAge, opt => opt.MapFrom(src => src.MinAge))
                .ForMember(dest => dest.MaxAge, opt => opt.MapFrom(src => src.MaxAge))
                .ForMember(dest => dest.MaxDistance, opt => opt.MapFrom(src => src.MaxDistance))
                .ForMember(dest => dest.PreferredGender, opt => opt.MapFrom(src => src.PreferredGender));
            CreateMap<PreferenceDTO, Preference>()
                .ForMember(dest => dest.MinAge, opt => opt.MapFrom(src => src.MinAge))
                .ForMember(dest => dest.MaxAge, opt => opt.MapFrom(src => src.MaxAge))
                .ForMember(dest => dest.MaxDistance, opt => opt.MapFrom(src => src.MaxDistance))
                .ForMember(dest => dest.PreferredGender, opt => opt.MapFrom(src => src.PreferredGender));

            // Swipe <-> SwipeDTO
            CreateMap<Swipe, SwipeDTO>()
                .ForMember(dest => dest.SwiperId, opt => opt.MapFrom(src => Convert.ToInt32(src.SwiperId)))
                .ForMember(dest => dest.TargetId, opt => opt.MapFrom(src => Convert.ToInt32(src.TargetId)))
                .ForMember(dest => dest.TargetAgree, opt => opt.MapFrom(src => src.TargetAgree));
            CreateMap<SwipeDTO, Swipe>()
                .ForMember(dest => dest.SwiperId, opt => opt.MapFrom(src => src.SwiperId.ToString()))
                .ForMember(dest => dest.TargetId, opt => opt.MapFrom(src => src.TargetId.ToString()))
                .ForMember(dest => dest.TargetAgree, opt => opt.MapFrom(src => src.TargetAgree));
        }
    }
}
