namespace BoardingHouseAPI.Service.Interface
{
    public interface IImageClientService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
