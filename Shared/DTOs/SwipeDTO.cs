namespace Shared.DTOs
{
    public class SwipeDto
    {
        public string SwiperId { get; set; } = string.Empty;
        public string TargetId { get; set; } = string.Empty;
        public bool? TargetAgree { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
