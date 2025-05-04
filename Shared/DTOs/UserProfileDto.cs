namespace Shared.DTOs
{
	public class UserProfileDto
	{
		public string UserName { get; set; } = null!;
		public int? Age { get; set; }
		public string? Gender { get; set; }
		public string? Bio { get; set; }
		public string? Email { get; set; }
	}
}
