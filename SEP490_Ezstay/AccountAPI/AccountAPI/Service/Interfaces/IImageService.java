package AccountAPI.Service.Interfaces;

public interface IImageService {

	public Task<String> UploadImageAsync(IFormFile aFile);
}