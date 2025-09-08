using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HouseLocationAPI.Service.Interface;
using Microsoft.AspNetCore.OData.Query;
using HouseLocationAPI.DTO.Request;

namespace HouseLocationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseLocationsController : ControllerBase
    {
        private readonly IHouseLocationService _houseLocationService;

        public HouseLocationsController(IHouseLocationService houseLocationService)
        {
            _houseLocationService = houseLocationService;
        }

        // GET: api/HouseLocations
        [HttpGet]
        [EnableQuery]
        public IQueryable<HouseLocationDTO> GetAll()
        {
            return _houseLocationService.GetAll();
        }

        // GET: api/HouseLocations/house/1111-1111-1111-1111
        [HttpGet("house/{houseId}")]
        [EnableQuery]
        public IQueryable<HouseLocationDTO> GetAllByHouseId(Guid houseId)
        {
            return _houseLocationService.GetByHouseId(houseId);
        }

        // GET: api/HouseLocations/1111-1111-1111-1111
        [HttpGet("{id}")]
        public async Task<ActionResult<HouseLocationDTO>> GetById(Guid id)
        {
            try
            {
                var houseLocation = await _houseLocationService.GetByIdAsync(id);                
                return Ok(houseLocation);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/HouseLocations
        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult<HouseLocationDTO>> Create([FromBody] CreateHouseLocationDTO dto)
        {
            try
            {
                var response = await _houseLocationService.CreateAsync(dto);
                if (!response.IsSuccess)
                {
                    return BadRequest(new { message = response.Message });
                }
                return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/HouseLocations/1111-1111-1111-1111
        [HttpPut("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHouseLocationDTO dto)
        {
            try
            {
                var response = await _houseLocationService.UpdateAsync(id, dto);
                if (!response.IsSuccess)
                {
                    return BadRequest(new { message = response.Message });
                }
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/HouseLocations/1111-1111-1111-1111
        [HttpDelete("{id}")]
        [Authorize (Roles = "Owner")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var response = await _houseLocationService.DeleteAsync(id);
                if (!response.IsSuccess)
                {
                    return BadRequest(new { message = response.Message });
                }
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
