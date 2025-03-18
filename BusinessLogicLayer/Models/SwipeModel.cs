namespace BusinessLayer.Models
{
    public class SwipeModel
    {
        public int SwiperId { get; set; }
        public int TargetId { get; set; }
        public bool? TargetAgree { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
