namespace ContractAPI.Services.Interfaces;

public interface IImageClientService
{
    Task<string> UploadImage(IFormFile file);
    Task<List<string>> UploadMultipleImage(IFormFileCollection files);
}