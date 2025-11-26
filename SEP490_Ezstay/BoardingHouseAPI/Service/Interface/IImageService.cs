namespace BoardingHouseAPI.Service.Interface
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
        // upload multiple images
        Task<List<string>> UploadMultipleImagesAsync(IFormFileCollection files);
    }
}
