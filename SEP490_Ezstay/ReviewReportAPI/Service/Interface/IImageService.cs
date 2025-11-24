namespace ReviewReportAPI.Service.Interface;

public interface IImageService
{
    Task<List<string>> UploadMultipleImage(IFormFileCollection files);
}