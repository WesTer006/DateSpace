using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Entities
{
	public class AppUser : IdentityUser
	{
		public int Age { get; set; }
		public required string Gender { get; set; }
		public string? Bio { get; set; }
		public DateTime CreatedAt { get; set; }

		public ICollection<Photo>? Photos { get; set; }
		public Preference? Preference { get; set; }
		public ICollection<Swipe>? SwipesAsSwiper { get; set; }
		public ICollection<Swipe>? SwipesAsTarget { get; set; }
		public ICollection<Message>? SentMessages { get; set; }
		public ICollection<Message>? ReceivedMessages { get; set; }
		public Location? Location { get; set; }
	}
}
