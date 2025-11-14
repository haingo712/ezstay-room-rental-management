using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmenityAPI.DTO.Request;
using Shared.DTOs.Amenities.Responses;
using AmenityAPI.Models;
using AmenityAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

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
        
        [HttpGet]
        [EnableQuery]
        public IQueryable<AmenityResponse> GetAll()
        {
            return  _amenityService.GetAll();
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var amenity = await _amenityService.GetById(id);
                    return Ok(amenity);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        
        [HttpPut("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Put(Guid id, [FromForm] UpdateAmenity request)
        {
            try
            {
                var updateAmentity =  await _amenityService.Update(id, request);
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
        public async Task<IActionResult> Post([FromForm] CreateAmenity request)
        {
            try
            {
                var createAmentity =   await  _amenityService.Add(request);
                if (!createAmentity.IsSuccess)
                {
                    return BadRequest(new { message = createAmentity.Message });
                }
                return CreatedAtAction("GetById", new { id = createAmentity.Data.Id }, createAmentity);
            }
            catch (Exception e)
            {
                return Conflict(new { message = e.Message }); 
            }
          
        }
      
        [HttpDelete("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            { 
                var amentity= await _amenityService.Delete( id);
                if (!amentity.IsSuccess)
                {
                    return BadRequest(new { message = amentity.Message });
                }
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

    }
}
