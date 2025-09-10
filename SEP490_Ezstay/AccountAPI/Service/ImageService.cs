using AccountAPI.Service.Interfaces;

namespace AccountAPI.Service
{
    public class ImageService : IImageService
    {
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            // TODO: call ImageAPI, nhận lại URL
            await Task.Delay(50);
            return "https://cdn.myapp.com/images/" + Guid.NewGuid() + Path.GetExtension(file.FileName);
        }
    }
}
