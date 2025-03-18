namespace BusinessLayer.Models
{
    public class AppUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<PhotoModel> Photos { get; set; } = new();
        public PreferenceModel? Preference { get; set; }
        public LocationModel? Location { get; set; }
        public List<SwipeModel> SwipesAsSwiper { get; set; } = new();
        public List<SwipeModel> SwipesAsTarget { get; set; } = new();
        public List<MessageModel> SentMessages { get; set; } = new();
        public List<MessageModel> ReceivedMessages { get; set; } = new();
    }
}
