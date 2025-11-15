using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAPI.DTO.Response;
using ServiceAPI.DTO.Resquest;
using ServiceAPI.Model;
using ServiceAPI.Service.Interfaces;

namespace ServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceItemService _service;

        public ServiceController(IServiceItemService service)
        {
            _service = service;
        }

        // POST: api/service/add
        [HttpPost("add")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateService([FromBody] ServiceItemRequestDto request)
        {
            try
            {
                var result = await _service.CreateServiceAsync(request);
                return Ok(new
                {
                    message = "Service created successfully",
                    data = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: api/service/all
        [HttpGet("all")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> GetAllServices()
        {
            var result = await _service.GetAllServicesAsync();
            return Ok(result);
        }

        // GET: api/service/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            var result = await _service.GetServiceByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Service not found" });
            return Ok(result);
        }

        // PUT: api/service/update/{id}
        [HttpPut("update/{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> UpdateService(Guid id, [FromBody] ServiceItemRequestDto updatedService)
        {
            try
            {
                await _service.UpdateServiceAsync(id, updatedService);
                return Ok(new { message = "Service updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/service/delete/{id}
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            await _service.DeleteServiceAsync(id);
            return Ok(new { message = "Service deleted successfully" });
        }
    }
}
