namespace DataAccessLayer.Entities
{
	public class Swipe
	{
		public required string SwiperId { get; set; }
		public required string TargetId { get; set; }
		public bool? TargetAgree { get; set; }
		public DateTime CreatedAt { get; set; }

		public required AppUser Swiper { get; set; }
		public required AppUser Target { get; set; }
	}
}
