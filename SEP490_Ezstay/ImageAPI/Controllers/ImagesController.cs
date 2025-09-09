using Microsoft.AspNetCore.Mvc;
using ImageAPI.DTO;
using ImageAPI.DTO.Request;
using ImageAPI.Service;
using ImageAPI.Service.Interface;

namespace ImageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _service;

        public ImagesController(IImageService service)
        {
            _service = service;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadDTO dto)
        {
            var result = await _service.UploadAsync(dto);
            return Ok(result);
        }

     
       

       
    }
}
