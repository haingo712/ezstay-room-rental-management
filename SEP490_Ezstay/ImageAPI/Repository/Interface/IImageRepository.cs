using ImageAPI.Models;

namespace ImageAPI.Repository.Interface
{
    public interface IImageRepository
    {
        Task<Image> AddAsync(Image entity);
    
    }
}
