using AutoMapper;
using ImageAPI.DTO.Request;
using ImageAPI.Models;

namespace ImageAPI.Profiles
{
    public class ImageProfile:Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageDTO>().ReverseMap();
        }
    }
}
