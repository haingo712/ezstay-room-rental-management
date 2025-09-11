using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmenityAPI.DTO.Request;
using AmenityAPI.Models;
using AmenityAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;

namespace AmenityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService _amenityService;
        private readonly ITokenService _tokenService;

        public AmenityController(IAmenityService amenityService, ITokenService tokenService)
        {
            _amenityService = amenityService;
            _tokenService = tokenService;
        }
        [HttpGet("ByStaffId/odata/")]
        [Authorize(Roles = "Staff")]
        [EnableQuery]
        public IQueryable<AmenityDto> GetAmenitiesByOwnerIdOdata( )
        {
            var staffId = _tokenService.GetUserIdFromClaims(User);
            return _amenityService.GetAllByStaffIdAsQueryable(staffId);
        }
        
        [HttpGet("ByStaffId")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<AmenityDto>> GetAmenitiesByStaffId()
        {
            var staffId = _tokenService.GetUserIdFromClaims(User);
            return Ok( await _amenityService.GetAllByStaffId(staffId));
        }
        [HttpGet("odata")]
        [EnableQuery]
        public IQueryable<AmenityDto> GetAmenitiesOdata()
        {
            return  _amenityService.GetAllAsQueryable();
        }
        
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public  async Task<ActionResult<AmenityDto>> GetAmenities()
        {
            return Ok(await _amenityService.GetAll());
        }
      
        // GET: api/Amenity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Amenity>> GetAmenity(Guid id)
        {
            try
            {
                var amenity = await _amenityService.GetByIdAsync(id);
                    return Ok(amenity);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        // PUT: api/Amenity/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> PutAmenity(Guid id, UpdateAmenityDto request)
        {
            try
            {
                var accountId = _tokenService.GetUserIdFromClaims(User);
                var updateAmentity =  await _amenityService.UpdateAsync(accountId ,id, request);
                if (!updateAmentity.IsSuccess)
                {
                    return BadRequest(new { message = updateAmentity.Message });
                }
                return Ok(updateAmentity);
           }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<AmenityDto>> PostAmenity(CreateAmenityDto request)
        {
            try
            {
                var staffId = _tokenService.GetUserIdFromClaims(User);
                var createAmentity =   await  _amenityService.AddAsync(staffId, request);
                if (!createAmentity.IsSuccess)
                {
                    return BadRequest(new { message = createAmentity.Message });
                }
                return CreatedAtAction("GetAmenity", new { id = createAmentity.Data.Id }, createAmentity);
            }
            catch (Exception e)
            {
                return Conflict(new { message = e.Message }); 
            }
          
        }
        // DELETE: api/Amenity/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteAmenity(Guid id)
        {
            try
            { 
               // var staffId = _tokenService.GetUserIdFromClaims(User);
                await _amenityService.DeleteAsync( id);
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

    }
}
