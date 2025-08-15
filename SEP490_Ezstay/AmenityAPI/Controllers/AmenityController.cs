using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmenityAPI.Data;
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

        public AmenityController(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }
      //  [Authorize(Roles = "Owner")]
        [HttpGet("ByOwnerId/{ownerId}")]
        [EnableQuery]
        public IQueryable<AmenityDto> GetAmenitiesByOwnerId(Guid ownerId)
        {
           
            return _amenityService.GetAllByOwnerId(ownerId);
        }
        
        [HttpGet("DistinctNames")]
        public async Task<ActionResult<IEnumerable<string>>> GetDistinctAmenityNames()
        {
            var names = await _amenityService.GetAllDistinctNameAsync();
            return Ok(names);
        }

        // [HttpGet]
        // [EnableQuery]
        // public IQueryable<AmenityDto> GetAmenities()
        // {
        //     return _amenityService.GetAll();
        // }
        [HttpGet]
        public async Task<ActionResult<AmenityDto>> GetAmenities()
        {
            return Ok(await _amenityService.GetAll());
        }

        // GET: api/Amenity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Amenity>> GetAmenity(int id)
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
   //     [Authorize(Roles = "Owner")]
        public async Task<IActionResult> PutAmenity(int id, UpdateAmenityDto request)
        {
            try
            {
                var updateAmentity =  await _amenityService.UpdateAsync(id, request);
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
      //  [Authorize(Roles = "Owner")]
        public async Task<ActionResult<AmenityDto>> PostAmenity(CreateAmenityDto request)
        {
            try
            {
                var createAmentity =   await  _amenityService.AddAsync(request);
                if (!createAmentity.IsSuccess)
                {
                    return BadRequest(new { message = createAmentity.Message });
                }
                return CreatedAtAction("GetAmenity", new { id = createAmentity.Data.AmenityId }, createAmentity);
            }
            catch (Exception e)
            {
                return Conflict(new { message = e.Message }); 
            }
          
        }
        // DELETE: api/Amenity/5
        [HttpDelete("{id}")]
    //    [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteAmenity(int id)
        {
            try
            { 
                await _amenityService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

    }
}
