namespace BusinessLayer.Models
{
    public class PreferenceModel
    {
        public int Id { get; set; }
        public string PreferredGender { get; set; } = string.Empty;
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int MaxDistance { get; set; }
    }
}
