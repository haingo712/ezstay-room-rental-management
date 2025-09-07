using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
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
        
        // get all k có phân quyền ai cũng dc
        [HttpGet]
        [EnableQuery]
        public IQueryable<RoomAmenityDto> GetRoomAmenities( )
        {
            return  _roomAmenityService.GetAll();
        }
        
        // GET: api/RoomAmenity
        //  [Authorize(Roles = "Owner")]
        [HttpGet("/Odata/ByRoomId/{roonId}")]
        [EnableQuery]
        public IQueryable<RoomAmenityDto> GetRoomAmenitiesByRoomIdOdata(Guid roonId)
        {
            return  _roomAmenityService.GetAllByRoomId(roonId);
        }
        
        // GET: api/RoomAmenity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomAmenityDto>> GetRoomAmenity(Guid id)
        {
            var roomAmenity = await _roomAmenityService.GetByIdAsync(id);
            return Ok(roomAmenity);
        }
        [HttpGet("ByRoomId/{roomId}")]
        public async Task<ActionResult<RoomAmenityDto>> GetRoomAmenitiesByRoomId(Guid roomId)
        {
            var roomAmenity = await _roomAmenityService.GetRoomAmenitiesByRoomIdAsync(roomId);
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
        // Add: api/RoomAmenity/5/Amenity
        [HttpPost("{roomId}/Amenity")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> PostRoomAmenity(Guid roomId, CreateRoomAmenityDto roomAmenity)
        {
           var createdRoomAmenity = await _roomAmenityService.AddAsync( roomId,roomAmenity);
           if (!createdRoomAmenity.IsSuccess ) 
           {
               return BadRequest(new { message = createdRoomAmenity.Message });
           }
            return CreatedAtAction("GetRoomAmenity", new { id = roomAmenity.AmenityId }, roomAmenity);
        }
        
        // DELETE: api/RoomAmenity/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteRoomAmenity(Guid id)
        {
            await _roomAmenityService.DeleteAsync(id);
            return NoContent();
        }
    }
}
