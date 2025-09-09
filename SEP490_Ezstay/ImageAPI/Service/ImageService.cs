using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ImageAPI.DTO;
using ImageAPI.DTO.Request;
using ImageAPI.Models;
using ImageAPI.Repository;
using ImageAPI.Repository.Interface;
using ImageAPI.Service.Interface;
using ImageAPI.Validators;

namespace ImageAPI.Service
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _repo;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public ImageService(IImageRepository repo, IMapper mapper, Cloudinary cloudinary)
        {
            _repo = repo;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        public async Task<ImageDTO> UploadAsync(ImageUploadDTO dto)
        {
            ImageValidator.Validate(dto.File);
            await using var stream = dto.File.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(dto.File.FileName, stream),
                PublicId = Guid.NewGuid().ToString(),
                Folder = "ezstay"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Upload failed: " + uploadResult.Error?.Message);

            var entity = new Image
            {

                Url = uploadResult.SecureUrl.AbsoluteUri,

            };

            await _repo.AddAsync(entity);

            return _mapper.Map<ImageDTO>(entity);
        }


    }
}
