namespace DateSpaceWebAPI.DTOs
{
	public class UserDto
	{
		public required string UserName { get; set; }
		public int Age { get; set; }
		public required string Gender { get; set; }
		public string? Bio { get; set; }
		public DateTime CreatedAt { get; set; }
		public required string Email { get; set; }
		public required string Password { get; set; }

	}

}
