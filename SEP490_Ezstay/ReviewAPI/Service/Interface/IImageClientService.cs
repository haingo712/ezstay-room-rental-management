namespace ReviewAPI.Service.Interface;

public interface IImageClientService
{
    Task<List<string>> UploadMultipleImage(IFormFileCollection files);
}