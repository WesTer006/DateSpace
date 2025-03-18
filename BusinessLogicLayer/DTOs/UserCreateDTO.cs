namespace BusinessLogicLayer.DTOs
{
    public class UserCreateDTO
    {
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? Bio { get; set; }
    }
}
