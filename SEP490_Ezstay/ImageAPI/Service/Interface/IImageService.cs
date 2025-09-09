using ImageAPI.DTO.Request;

namespace ImageAPI.Service.Interface
{
    public interface IImageService
    {
        Task<ImageDTO> UploadAsync(ImageUploadDTO dto);
 
    }
}
