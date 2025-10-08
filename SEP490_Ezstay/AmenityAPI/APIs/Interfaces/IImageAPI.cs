namespace AmenityAPI.APIs.Interfaces;

public interface IImageAPI
{ 
    Task<string> UploadImage(IFormFile file);
}