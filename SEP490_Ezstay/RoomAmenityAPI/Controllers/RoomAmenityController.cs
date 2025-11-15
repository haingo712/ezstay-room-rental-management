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
        public IQueryable<RoomAmenityResponse> GetAll( )
        {
            return  _roomAmenityService.GetAll();
        }
        
        [Authorize(Roles = "Owner")]
        [HttpGet("/room-id/{roonId}")]
        [EnableQuery]
        public IQueryable<RoomAmenityResponse> GetRoomAmenitiesByRoomId(Guid roonId)
        {
            return  _roomAmenityService.GetAllByRoomId(roonId);
        }
        [Authorize(Roles = "Owner")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomAmenityResponse>> GetById(Guid id)
        {
            var roomAmenity = await _roomAmenityService.GetById(id);
            return Ok(roomAmenity);
        }
        // // [Authorize(Roles = "Owner")]
        // [HttpGet("byRoomId/{roomId}")]
        // public async Task<ActionResult<RoomAmenityResponse>> GetRoomAmenitiesByRoomId(Guid roomId)
        // {
        //     var roomAmenity = await _roomAmenityService.GetRoomAmenitiesByRoomIdAsync(roomId);
        //     return Ok(roomAmenity);
        // }
        [HttpPut("{roomId}/amenities")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> SetRoomAmenities(Guid roomId, List<CreateRoomAmenity> roomAmenity)
        {
            var createdRoomAmenity = await _roomAmenityService.UpdateRoomAmenities( roomId,roomAmenity);
            if (!createdRoomAmenity.IsSuccess ) 
            {
                return BadRequest(new { message = createdRoomAmenity.Message });
            }
           // return Ok(createdRoomAmenity);
             return CreatedAtAction("GetById", new { roomId = roomId }, createdRoomAmenity.Data);
        }
        // amenity api dungf dder check khi delete
        [HttpGet("check-by-amenity/{amenityId}")]
        public async Task<ActionResult<bool>> CheckAmenity(Guid amenityId)
        {
            var isUsed = await _roomAmenityService.CheckAmenity(amenityId);
            return Ok(isUsed);
        }
        // [HttpDelete("room-id/{roomId}")]
        // public async Task<IActionResult> DeleteAmenityByRoomId(Guid roomId)
        // {
        //     var result = await _roomAmenityService.DeleteAmenityByRoomId(roomId);
        //     return Ok(result);
        // }
    }
}
