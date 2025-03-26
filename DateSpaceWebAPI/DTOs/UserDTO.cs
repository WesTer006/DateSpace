namespace DateSpaceWebAPI.DTOs
{
	public class UserDTO
	{
		public string UserName { get; set; }
		public int Age { get; set; }
		public string Gender { get; set; }
		public string? Bio { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }

	}

}
