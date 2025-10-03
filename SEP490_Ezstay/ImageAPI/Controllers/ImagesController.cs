using ImageAPI.DTO.Request;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly ImageService _ipfsService;

    public ImagesController(ImageService ipfsService)
    {
        _ipfsService = ipfsService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] ImageUploadDTO dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("File không hợp lệ");

        var url = await _ipfsService.UploadAsync(dto.File);

        return Ok(new { Url = url });
    }
     [HttpPost("upload-multiple")]
    public async Task<IActionResult> UploadMultiple([FromForm] ImageUploadMultipleDTO request)
    {
        if (request.Files == null || request.Files.Count == 0)
            return BadRequest("No files uploaded.");

        var urls = await _ipfsService.UploadMultipleAsync(request.Files);
        return Ok(new { Urls = urls });
    }
}
