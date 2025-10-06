namespace AmenityAPI.APIs.Interfaces;

public interface IImageAPI
{ 
    Task<string> UploadImageAsync(IFormFile file);
}