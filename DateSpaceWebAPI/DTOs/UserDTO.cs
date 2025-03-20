namespace DateSpaceWebAPI.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? Bio { get; set; }

        public List<PhotoDTO> Photos { get; set; } = new();
        public PreferenceDTO? Preference { get; set; }
        public LocationDTO? Location { get; set; }
    }
}
