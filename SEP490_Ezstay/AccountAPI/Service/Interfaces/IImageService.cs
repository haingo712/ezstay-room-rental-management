namespace AccountAPI.Service.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);

    }
}
