using ImageAPI.DTO.Request;
using ImageAPI.Models;

namespace ImageAPI.Service.Interface
{
    public interface IImageService
    {
        Task<string> UploadAsync(ImageUploadDTO dto);

    }
}
