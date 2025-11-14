namespace AmenityAPI.Service.Interface;

public interface IImageService
{ 
    Task<string> UploadImage(IFormFile file);
}