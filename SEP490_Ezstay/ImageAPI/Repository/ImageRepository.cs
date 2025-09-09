
using ImageAPI.Models;
using ImageAPI.Repository.Interface;

namespace ImageAPI.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly List<Image> _storage = new(); // demo, có thể thay bằng DbContext

        public async Task<Image> AddAsync(Image entity)
        {
            _storage.Add(entity);
            return await Task.FromResult(entity);
        }

     
    }
}
