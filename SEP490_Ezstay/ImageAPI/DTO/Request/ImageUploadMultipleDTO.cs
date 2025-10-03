namespace ImageAPI.DTO.Request
{
    public class ImageUploadMultipleDTO
    {
        public IFormFileCollection Files { get; set; } = default!;
    }
}
