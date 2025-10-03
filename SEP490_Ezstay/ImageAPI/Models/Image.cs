namespace ImageAPI.Models
{
    public class Image
    {
       
        
            public Guid Id { get; set; }
            public string Url { get; set; } = string.Empty; // link ảnh từ Filebase
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    }
}
