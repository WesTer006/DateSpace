namespace DateSpaceWebAPI.DTOs
{
    public class MessageDto
    {
        public string SenderId { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
        public string MessageText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
