namespace RoomAPI.Service.Interface;

public interface IImageClientService
{
    Task<List<string>> UploadMultipleImage(IFormFileCollection files);
}