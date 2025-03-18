namespace DataAccessLayer.Entities
{
	public class Photo
	{
		public int Id { get; set; }
		public required string UserId { get; set; }
		public required string Url { get; set; }
		public bool IsMain { get; set; }

		public required AppUser User { get; set; }
	}
}
