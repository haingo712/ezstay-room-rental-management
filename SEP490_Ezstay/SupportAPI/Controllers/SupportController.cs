using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupportAPI.DTO.Request;
using SupportAPI.Service;
using SupportAPI.Service.Interfaces;

namespace SupportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly ISupportService _service;

        public SupportController(ISupportService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles =("Staff"))]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateSupportRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = ("Staff"))]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateSupportStatusRequest request)
        {
            var result = await _service.UpdateStatusAsync(id, request);
            return Ok(result);
        }
    }
}
