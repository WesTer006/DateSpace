namespace BusinessLayer.Models
{
    public class PhotoModel
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool IsMain { get; set; }
    }
}
