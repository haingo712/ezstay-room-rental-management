namespace ImageAPI.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string Url { get; set; } = string.Empty; // link ảnh Cloudinary
        public bool IsPrimary { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
