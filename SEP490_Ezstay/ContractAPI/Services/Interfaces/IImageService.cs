namespace ContractAPI.Services.Interfaces;

public interface IImageService
{
    Task<string> UploadImage(IFormFile file);
    Task<List<string>> UploadMultipleImage(IFormFileCollection files);
}