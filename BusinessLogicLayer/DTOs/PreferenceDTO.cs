namespace BusinessLogicLayer.DTOs
{
    public class PreferenceDTO
    {
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int MaxDistance { get; set; }
        public string PreferredGender { get; set; } = string.Empty;
    }
}
