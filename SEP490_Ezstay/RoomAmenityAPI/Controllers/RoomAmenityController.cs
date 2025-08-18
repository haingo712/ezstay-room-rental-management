using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using RoomAmenityAPI.Data;
using RoomAmenityAPI.DTO.Request;
using RoomAmenityAPI.DTO.Response;
using RoomAmenityAPI.Service.Interface;
using RoomAmenityAPI.Model;

namespace RoomAmenityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomAmenityController : ControllerBase
    {
        private readonly IRoomAmenityService _roomAmenityService;

        public RoomAmenityController(IRoomAmenityService roomAmenityService)
        {
            _roomAmenityService = roomAmenityService;
        }

        // GET: api/RoomAmenity
   //     [Authorize(Roles = "Owner")]
        [HttpGet("ByRoomId/{roonId}")]
        [EnableQuery]
        public IQueryable<RoomAmenityDto> GetRoomAmenitiesByRoomId(Guid roonId)
        {
            return  _roomAmenityService.GetAllByRoomId(roonId);
        }
        [HttpGet]
        [EnableQuery]
        public IQueryable<RoomAmenityDto> GetRoomAmenities( )
        {
            return  _roomAmenityService.GetAll();
        }

        // GET: api/RoomAmenity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomAmenityDto>> GetRoomAmenity(Guid id)
        {
            var roomAmenity = await _roomAmenityService.GetByIdAsync(id);
            return Ok(roomAmenity);
        }
      [HttpPut("{id}")]
      [Authorize(Roles = "Owner")]
        public async Task<IActionResult> PutRoomAmenity(Guid id, UpdateRoomAmenityDto roomAmenity)
        {
        var createdRoomAmenity=   await   _roomAmenityService.UpdateAsync(id,roomAmenity);
           if (!createdRoomAmenity.IsSuccess ) 
           {
               return BadRequest(new { message = createdRoomAmenity.Message });
           }
           return Ok(createdRoomAmenity);
        }
        [HttpPost]
    //    [Authorize(Roles = "Owner")]
        public async Task<IActionResult> PostRoomAmenity(CreateRoomAmenityDto roomAmenity)
        {
        
           var createdRoomAmenity = await _roomAmenityService.AddAsync(roomAmenity);
           if (!createdRoomAmenity.IsSuccess ) 
           {
               return BadRequest(new { message = createdRoomAmenity.Message });
           }
            return CreatedAtAction("GetRoomAmenity", new { id = roomAmenity.AmenityId }, roomAmenity);
        }
        // DELETE: api/RoomAmenity/5
        [HttpDelete("{id}")]
      //  [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteRoomAmenity(Guid id)
        {
            await _roomAmenityService.DeleteAsync(id);
            return NoContent();
        }
    }
}
