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
        [HttpGet]
        [EnableQuery]
        [Authorize(Roles = "Staff")]
        public IQueryable<RoomAmenityResponseDto> GetRoomAmenities( )
        {
            return  _roomAmenityService.GetAll();
        }
        // GET: api/RoomAmenity
          [Authorize(Roles = "Owner")]
        //  uu tien lam 
        [HttpGet("/Odata/ByRoomId/{roonId}")]
        [EnableQuery]
        public IQueryable<RoomAmenityResponseDto> GetRoomAmenitiesByRoomIdOdata(Guid roonId)
        {
            return  _roomAmenityService.GetAllByRoomId(roonId);
        }
        // GET: api/RoomAmenity/5
        //  ****
        [Authorize(Roles = "Owner")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomAmenityResponseDto>> GetRoomAmenity(Guid id)
        {
            var roomAmenity = await _roomAmenityService.GetByIdAsync(id);
            return Ok(roomAmenity);
        }
        //  uu tien lam 
        // [Authorize(Roles = "Owner")]
        [HttpGet("ByRoomId/{roomId}")]
        public async Task<ActionResult<RoomAmenityResponseDto>> GetRoomAmenitiesByRoomId(Guid roomId)
        {
            var roomAmenity = await _roomAmenityService.GetRoomAmenitiesByRoomIdAsync(roomId);
            return Ok(roomAmenity);
        }
        
        [HttpPost("{roomId}/Amenity")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> PostRoomAmenity(Guid roomId, List<CreateRoomAmenityDto> roomAmenity)
        {
            var createdRoomAmenity = await _roomAmenityService.AddAsync( roomId,roomAmenity);
            if (!createdRoomAmenity.IsSuccess ) 
            {
                return BadRequest(new { message = createdRoomAmenity.Message });
            }
            return Ok(createdRoomAmenity);
            // return CreatedAtAction("GetRoomAmenity", new { roomId = roomId }, createdRoomAmenity.Data);
        }
    }
}
