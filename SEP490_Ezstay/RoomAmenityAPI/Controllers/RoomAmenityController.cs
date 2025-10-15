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
using Shared.DTOs.RoomAmenities.Responses;
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
        public IQueryable<RoomAmenityResponse> GetRoomAmenities( )
        {
            return  _roomAmenityService.GetAll();
        }
        // GET: api/RoomAmenity
        [Authorize(Roles = "Owner")]
        [HttpGet("/odata/byRoomId/{roonId}")]
        [EnableQuery]
        public IQueryable<RoomAmenityResponse> GetRoomAmenitiesByRoomIdOdata(Guid roonId)
        {
            return  _roomAmenityService.GetAllByRoomId(roonId);
        }
        // GET: api/RoomAmenity/5
        [Authorize(Roles = "Owner")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomAmenityResponse>> GetRoomAmenity(Guid id)
        {
            var roomAmenity = await _roomAmenityService.GetByIdAsync(id);
            return Ok(roomAmenity);
        }
        //  uu tien lam 
        // [Authorize(Roles = "Owner")]
        [HttpGet("byRoomId/{roomId}")]
        public async Task<ActionResult<RoomAmenityResponse>> GetRoomAmenitiesByRoomId(Guid roomId)
        {
            var roomAmenity = await _roomAmenityService.GetRoomAmenitiesByRoomIdAsync(roomId);
            return Ok(roomAmenity);
        }
        
        [HttpPost("{roomId}/amenity")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> PostRoomAmenity(Guid roomId, List<CreateRoomAmenity> roomAmenity)
        {
            var createdRoomAmenity = await _roomAmenityService.AddAsync( roomId,roomAmenity);
            if (!createdRoomAmenity.IsSuccess ) 
            {
                return BadRequest(new { message = createdRoomAmenity.Message });
            }
            return Ok(createdRoomAmenity);
            // return CreatedAtAction("GetRoomAmenity", new { roomId = roomId }, createdRoomAmenity.Data);
        }
        [HttpGet("check-by-amenity/{amenityId}")]
        public async Task<ActionResult<bool>> CheckAmenityUsage(Guid amenityId)
        {
            var isUsed = await _roomAmenityService.CheckAmenity(amenityId);
            return Ok(isUsed);
        }

    }
}
