namespace DataAccessLayer.Entities
{
	public class Message
	{
		public int Id { get; set; }
		public required string SenderId { get; set; }
		public required string ReceiverId { get; set; }
		public required string MessageText { get; set; }
		public DateTime CreatedAt { get; set; }

		public required AppUser Sender { get; set; }
		public required AppUser Receiver { get; set; }
	}
}
