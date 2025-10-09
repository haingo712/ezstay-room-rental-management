namespace ContractAPI.APIs.Interfaces;

public interface IImageAPI
{ 
    Task<string> UploadImage(IFormFile file);
}